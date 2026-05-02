using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SkillSnap.Client;
using SkillSnap.Client.Services;
using SkillSnap.Client.Auth;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SkillSnap.Client.Services.State;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ----------------------------------------------------
// LOCAL STORAGE + AUTH
// ----------------------------------------------------
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

// Register CustomAuthStateProvider correctly
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<CustomAuthStateProvider>());

// ----------------------------------------------------
// HTTP CLIENT (NO TOKEN LOADING HERE)
// ----------------------------------------------------
builder.Services.AddScoped(sp =>
{
    return new HttpClient
    {
        BaseAddress = new Uri("http://localhost:5158/")
    };
});

// ----------------------------------------------------
// APP SERVICES
// ----------------------------------------------------
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<SkillService>();
builder.Services.AddScoped<ProfileService>();

// ----------------------------------------------------
// CLIENT-SIDE STATE MANAGEMENT
// ----------------------------------------------------
builder.Services.AddScoped<AppState>();

await builder.Build().RunAsync();
