using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Attendiia;
using Attendiia.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Attendiia.Services.Interface;
using Attendiia.Services;
using Firebase.Auth;
using Attendiia.Stores;
using System.Security.Claims;
using Firebase.Auth.Providers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//hard coding
builder.Services.AddHttpClient("RefreshToken", client => client.BaseAddress = new Uri("https://securetoken.googleapis.com/v1/token?key=AIzaSyCIKg9EhtaRp4Esc1Qevbq5E2y_muCTnmI"));
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("RefreshToken"));

//configuration check
builder.Services.AddScoped(p =>
{
    FirebaseAuthenticationSettings? firebaseSettings = p.GetRequiredService<IConfiguration>().GetSection("FirebaseSettings").Get<FirebaseAuthenticationSettings>();
    if (firebaseSettings == null)
    {
        throw new ArgumentNullException("Firebase settings is null or not found. Please give us firebase settings.");
    }
    if (string.IsNullOrEmpty(firebaseSettings.FirebaseApiKey))
    {
        throw new ArgumentNullException($"{nameof(firebaseSettings.FirebaseApiKey)} is null or empty.");
    }
    if (string.IsNullOrEmpty(firebaseSettings.FirebaseAuthDomain))
    {
        throw new ArgumentNullException($"{nameof(firebaseSettings.FirebaseAuthDomain)} is null or empty.");
    }
    return firebaseSettings;
});
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy(AuthorizePolicy.DEFAULT, policy =>
    {
        policy.RequireClaim(ClaimTypes.NameIdentifier);
    });
    options.AddPolicy(AuthorizePolicy.REQUIRE_GROUP, policy =>
    {
        policy.RequireClaim(ClaimTypes.NameIdentifier);
        policy.RequireClaim(ClaimTypes.UserData);
    });
});
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<FirebaseAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    p => p.GetRequiredService<FirebaseAuthenticationStateProvider>());
builder.Services.AddScoped<IAuthenticationService, FirebaseAuthenticationService>();
builder.Services.AddScoped<IFirebaseDatabaseService, FirebaseDatabaseService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IGroupUserService, GroupUserService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<UserGroupContainer>();
builder.Services.AddScoped<UserCredentialContainer>();
builder.Services.AddScoped<FirebaseAuthConfig>(a =>
{
    var firebaseSettings = a.GetService<FirebaseAuthenticationSettings>();
    if (firebaseSettings == null)
    {
        throw new InvalidOperationException("Unexpected error occured.");
    }
    return new FirebaseAuthConfig
    {
        ApiKey = firebaseSettings.FirebaseApiKey,
        AuthDomain = firebaseSettings.FirebaseAuthDomain,
        Providers = new[] {
            new EmailProvider()
        }
    };
});
builder.Services.AddScoped<IFirebaseAuthClient, FirebaseAuthClient>();
await builder.Build().RunAsync();