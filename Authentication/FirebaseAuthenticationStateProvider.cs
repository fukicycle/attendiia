using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using Attendiia.Services.Interface;
using Attendiia.Stores;
using Newtonsoft.Json;
using Attendiia.Models;

namespace Attendiia.Authentication;

public sealed class FirebaseAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;
    private readonly IGroupService _groupService;
    private readonly IGroupUserService _groupUserService;
    private readonly UserGroupContainer _userGroupContainer;
    private readonly ILogger<FirebaseAuthenticationStateProvider> _logger;

    public FirebaseAuthenticationStateProvider(
        HttpClient httpClient,
        ILocalStorageService localStorageService,
        IGroupService groupService,
        IGroupUserService groupUserService,
        UserGroupContainer userGroupContainer,
        ILogger<FirebaseAuthenticationStateProvider> logger)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
        _groupService = groupService;
        _groupUserService = groupUserService;
        _userGroupContainer = userGroupContainer;
        _logger = logger;
    }


    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? accessToken =
            await _localStorageService.GetItemAsync<string>(LocalStorageKey.ACCESS_TOKEN);
        LoginUserInfo? loginUserInfo =
            await _localStorageService.GetItemAsync<LoginUserInfo>(LocalStorageKey.USER_INFO);

        if (string.IsNullOrEmpty(accessToken) || loginUserInfo == null)
        {
            return new AuthenticationState(
                new ClaimsPrincipal(
                    new ClaimsIdentity()));
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

        List<Claim> claims =
        [
            new Claim(ClaimTypes.Email, loginUserInfo.Email),
            new Claim(ClaimTypes.Name,loginUserInfo.DisplayName),
        ];

        //get and set groups of authorized user.
        _userGroupContainer.Groups.Clear();
        _userGroupContainer.Groups.AddRange(await GetGroupsByEmailAsync(loginUserInfo.Email));
        GroupUser? groupUser = await _groupUserService.GetGroupUserIsCurrentAsync(loginUserInfo.Email);
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

    public async Task NotifySignIn(LoginUserInfo loginUserInfo, string accessToken)
    {
        await _localStorageService.SetItemAsync(LocalStorageKey.USER_INFO, loginUserInfo);
        await _localStorageService.SetItemAsync(LocalStorageKey.ACCESS_TOKEN, accessToken);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task NotifySignOut()
    {
        await _localStorageService.RemoveItemAsync(LocalStorageKey.USER_INFO);
        await _localStorageService.RemoveItemAsync(LocalStorageKey.ACCESS_TOKEN);
        if (_httpClient.DefaultRequestHeaders.Authorization != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private async Task<List<Group>> GetGroupsByEmailAsync(string email)
    {
        List<Group> groups = new List<Group>();
        try
        {
            List<GroupUser> groupUsers = await _groupUserService.GetGroupUsersByEmailAsync(email);
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
}