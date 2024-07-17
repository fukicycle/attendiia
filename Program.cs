using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazored.LocalStorage;
using Attendiia;
using Attendiia.Authentication;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//configuration check
builder.Services.AddScoped(p =>
{
    FirebaseSettings? firebaseSettings = p.GetRequiredService<IConfiguration>().GetSection("FirebaseSettings").Get<FirebaseSettings>();
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
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<FirebaseAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(
    p => p.GetRequiredService<FirebaseAuthenticationStateProvider>());
builder.Services.AddScoped<IAuthenticationService, FirebaseAuthenticationService>();

await builder.Build().RunAsync();
