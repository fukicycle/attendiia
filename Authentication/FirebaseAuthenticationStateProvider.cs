using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

namespace Attendiia.Authentication;

public sealed class FirebaseAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;

    public FirebaseAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _localStorageService = localStorageService;
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

        return new AuthenticationState(
            new ClaimsPrincipal(
                new ClaimsIdentity(
                    new Claim[]{
                        new Claim(ClaimTypes.Name,loginUserInfo.Email)
                    }, "apiAuth"
                )
            )
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
}