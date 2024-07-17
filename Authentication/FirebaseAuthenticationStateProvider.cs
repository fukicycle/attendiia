using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

namespace Attendiia.Authenticaion;

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
        string? userId =
            await _localStorageService.GetItemAsync<string>(LocalStorageKey.USER_ID);

        if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(userId))
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
                        new Claim(ClaimTypes.Name,userId)
                    }, "apiAuth"
                )
            )
        );
    }

    public async Task NotifySignIn(string userId, string accessToken)
    {
        await _localStorageService.SetItemAsync(LocalStorageKey.USER_ID, userId);
        await _localStorageService.SetItemAsync(LocalStorageKey.ACCESS_TOKEN, accessToken);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task NotifySignOut()
    {
        await _localStorageService.RemoveItemAsync(LocalStorageKey.USER_ID);
        await _localStorageService.RemoveItemAsync(LocalStorageKey.ACCESS_TOKEN);
        if (_httpClient.DefaultRequestHeaders.Authorization != null)
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}