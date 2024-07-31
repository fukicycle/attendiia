using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Attendiia.Services.Interface;
using Attendiia.Stores;
using Attendiia.Models;
using Firebase.Auth.Requests;
using Attendiia.Dto;
using System.Net.Http.Json;

namespace Attendiia.Authentication;

public sealed class FirebaseAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IGroupService _groupService;
    private readonly IGroupUserService _groupUserService;
    private readonly UserGroupContainer _userGroupContainer;
    private readonly UserCredentialContainer _userCredentialContainer;
    private readonly ILogger<FirebaseAuthenticationStateProvider> _logger;
    private readonly HttpClient _refreshTokenHttpClient;

    public FirebaseAuthenticationStateProvider(
        ILocalStorageService localStorageService,
        IGroupService groupService,
        IGroupUserService groupUserService,
        UserGroupContainer userGroupContainer,
        UserCredentialContainer userCredentialContainer,
        ILogger<FirebaseAuthenticationStateProvider> logger,
        HttpClient httpClient)
    {
        _localStorageService = localStorageService;
        _groupService = groupService;
        _groupUserService = groupUserService;
        _userGroupContainer = userGroupContainer;
        _userCredentialContainer = userCredentialContainer;
        _logger = logger;
        _refreshTokenHttpClient = httpClient;
    }


    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? idToken =
            await _localStorageService.GetItemAsync<string>(LocalStorageKey.ID_TOKEN);
        string? refreshToken =
            await _localStorageService.GetItemAsync<string>(LocalStorageKey.REFRESH_TOKEN);
        LoginUserInfo? loginUserInfo =
            await _localStorageService.GetItemAsync<LoginUserInfo>(LocalStorageKey.USER_INFO);
        DateTime? expires = await _localStorageService.GetItemAsync<DateTime>(LocalStorageKey.EXPIRES_DATE_TIME);

        if (string.IsNullOrEmpty(idToken) || string.IsNullOrEmpty(refreshToken) || loginUserInfo == null || expires == null)
        {
            return new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity()));
        }

        _userCredentialContainer.IdToken = idToken;
        _userCredentialContainer.RefreshToken = refreshToken;

        //check token lifetime
        if (DateTime.UtcNow >= expires)
        {
            if (!await RefreshTokenAsync(refreshToken))
            {
                return new AuthenticationState(
                    new ClaimsPrincipal(
                        new ClaimsIdentity()));
            }
        }

        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, loginUserInfo.Uid),
            new Claim(ClaimTypes.Name,loginUserInfo.DisplayName),
        ];

        //get and set groups of authorized user.
        _userGroupContainer.Groups.Clear();
        _userGroupContainer.Groups.AddRange(await GetGroupsByEmailAsync(loginUserInfo.Uid));
        GroupUser? groupUser = await _groupUserService.GetGroupUserIsCurrentAsync(loginUserInfo.Uid);
        //if contains groups, add claims.
        if (groupUser != default)
        {
            _userGroupContainer.CurrentGroupCode = groupUser.GroupCode;
            claims.Add(new Claim(ClaimTypes.UserData, _userGroupContainer.CurrentGroupCode));
        }

        return new AuthenticationState(
            new ClaimsPrincipal(new ClaimsIdentity(claims, "apiAuth"))
        );
    }

    public async Task NotifySignIn(LoginUserInfo loginUserInfo, string accessToken, string refreshToken, int expiresIn)
    {
        await _localStorageService.SetItemAsync(LocalStorageKey.USER_INFO, loginUserInfo);
        await _localStorageService.SetItemAsync(LocalStorageKey.ID_TOKEN, accessToken);
        await _localStorageService.SetItemAsync(LocalStorageKey.REFRESH_TOKEN, refreshToken);
        DateTime current = DateTime.UtcNow;
        DateTime expires = current.AddSeconds(expiresIn - 10);
        await _localStorageService.SetItemAsync(LocalStorageKey.EXPIRES_DATE_TIME, expires);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task NotifySignOut()
    {
        await _localStorageService.RemoveItemAsync(LocalStorageKey.USER_INFO);
        await _localStorageService.RemoveItemAsync(LocalStorageKey.ID_TOKEN);
        await _localStorageService.RemoveItemAsync(LocalStorageKey.REFRESH_TOKEN);
        await _localStorageService.RemoveItemAsync(LocalStorageKey.EXPIRES_DATE_TIME);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private async Task<List<Group>> GetGroupsByEmailAsync(string uid)
    {
        List<Group> groups = new List<Group>();
        try
        {
            List<GroupUser> groupUsers = await _groupUserService.GetGroupUsersByUidAsync(uid);
            foreach (var groupUser in groupUsers)
            {
                groups.Add(await _groupService.GetGroupByCodeAsync(groupUser.GroupCode));
            }
        }
        catch (NotSupportedException e)
        {
            //TODO():error handling {e}
            _logger.LogError(e.Message);
        }
        return groups;
    }

    private async Task<bool> RefreshTokenAsync(string refreshToken)
    {
        try
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));
            parameters.Add(new KeyValuePair<string, string>("refresh_token", refreshToken));
            HttpResponseMessage httpResponseMessage = await _refreshTokenHttpClient.PostAsync("", new FormUrlEncodedContent(parameters));
            RefreshTokenResponseDto? refreshTokenResponseDto = await httpResponseMessage.Content.ReadFromJsonAsync<RefreshTokenResponseDto>();
            if (refreshTokenResponseDto == null)
            {
                return false;
            }

            _userCredentialContainer.IdToken = refreshTokenResponseDto.IdToken;
            _userCredentialContainer.RefreshToken = refreshTokenResponseDto.RefreshToken;

            //remove exists data
            await _localStorageService.RemoveItemAsync(LocalStorageKey.ID_TOKEN);
            await _localStorageService.RemoveItemAsync(LocalStorageKey.REFRESH_TOKEN);
            await _localStorageService.RemoveItemAsync(LocalStorageKey.EXPIRES_DATE_TIME);

            //update data
            await _localStorageService.SetItemAsync(LocalStorageKey.ID_TOKEN, refreshTokenResponseDto.IdToken);
            await _localStorageService.SetItemAsync(LocalStorageKey.REFRESH_TOKEN, refreshTokenResponseDto.RefreshToken);
            DateTime expires = DateTime.UtcNow.AddSeconds(refreshTokenResponseDto.ExpiresIn - 10);
            await _localStorageService.SetItemAsync(LocalStorageKey.EXPIRES_DATE_TIME, expires);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return false;
        }
        return true;
    }
}