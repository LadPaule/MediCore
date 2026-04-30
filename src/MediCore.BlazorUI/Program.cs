using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.DataProtection;

using MudBlazor.Services;

using MediCore.BlazorUI;
using MediCore.BlazorUI.Authentication;
using MediCore.BlazorUI.Services;

var builder = WebApplication.CreateBuilder(args);


//Todo: Core UI Services


builder.Services.AddRazorPages();

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();


//Todo: Authentication / Authorization


builder.Services.AddAuthorizationCore();

builder.Services.AddCascadingAuthenticationState();
//

builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => 
    sp.GetRequiredService<CustomAuthStateProvider>());


//Todo: Secure Browser Storage


builder.Services.AddScoped<ProtectedLocalStorage>();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/keys"))
    .SetApplicationName("MediCore_shared");

//Todo: API Communication


builder.Services.AddScoped(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();

    var baseAddress =
        config["HttpClient:BaseAddress"]
        ?? "http://api:8080/";

    return new HttpClient
    {
        BaseAddress = new Uri(baseAddress)
    };
});


//Todo: Application Services


builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ToastService>();

builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<PatientService>();
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<PharmacyService>();
builder.Services.AddScoped<DoctorService>();
builder.Services.AddScoped<PrescriptionService>();


//Todo: Docker compatibility


builder.WebHost.UseUrls("http://0.0.0.0:8080");


builder.Services.AddAntiforgery(options =>
{
    options.SuppressXFrameOptionsHeader = true;
});



var app = builder.Build();


//Todo: Middleware Pipeline


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

app.UseAntiforgery();


//Todo: Endpoint Mapping


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();