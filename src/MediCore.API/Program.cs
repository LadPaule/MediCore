using MediCore.Infrastructure.Data;
using MediCore.Infrastructure.Data.Seed;
using MediCore.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using MediCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MediCore.Application.Interfaces;
using MediCore.Application.Caching;
using Microsoft.AspNetCore.DataProtection;
using MediCore.Application.Services;
using MediCore.Application.Services.Analytics;
using MediCore.Infrastructure.Repositories;
using System.Text;

using Hangfire;
using Hangfire.MemoryStorage;

using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.AspNetCore.Mvc;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;


// Todo: Core Services


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
// builder.Services.AddRazorPages();

builder.Services.AddRazorPages(options =>
{
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
});
//Todo: Database


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        o =>
        {
            o.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorCodesToAdd: null);
        }));

// Todo: Identity


builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


//Todo: Authentication (JWT + Identity Cookies)

// builder.Services
//     .AddAuthentication(options =>
//     {
//         // CHANGE: Use JwtBearer as the default so the API returns 401 instead of a 302 redirect
//         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//         options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//     })
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = false,
//             ValidateAudience = false,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(
//                 Encoding.UTF8.GetBytes(configuration["jwt:Key"] ?? "SUPER_SECRET_KEY_12345"))
//         };
//         
//         // This prevents the "Unsafe attempt" browser error by ensuring 
//         // the API doesn't try to redirect to a non-existent login page.
//         options.Events = new JwtBearerEvents
//         {
//             OnChallenge = context =>
//             {
//                 context.HandleResponse();
//                 context.Response.StatusCode = 401;
//                 context.Response.ContentType = "application/json";
//                 return context.Response.WriteAsync("{\"error\": \"Unauthorized\"}");
//             }
//         };
//     });




// builder.Services
//     .AddAuthentication(options =>
//     {
//         options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
//         options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
//     })
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = false,
//             ValidateAudience = false,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new SymmetricSecurityKey(
//                 Encoding.UTF8.GetBytes(
//                     configuration["jwt:Key"]
                    // ?? throw new InvalidOperationException("JWT Key missing")
//                 ))
//         };
//     });

builder.Services.AddAuthorization();


//Todo: Data Protection


builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/keys"))
    .SetApplicationName("MediCore_shared");


//Todo: Redis Cache


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "MediCore:";
});


//Todo: Hangfire Background Jobs


builder.Services.AddHangfire(config =>
    config.UseMemoryStorage());

builder.Services.AddHangfireServer();


//Todo: Application Services


builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<RedisCacheService>();

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<PatientService>();

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<AppointmentService>();

builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddScoped<MedicalRecordService>();

builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<PrescriptionService>();

builder.Services.AddScoped<IPharmacyRepository, PharmacyRepository>();
builder.Services.AddScoped<PharmacyService>();

builder.Services.AddScoped<IHospitalAnalyticsRepository, HospitalAnalyticsRepository>();
builder.Services.AddScoped<HospitalAnalyticsService>();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:8080", "http://localhost:5073") 
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryTokenAttribute());
});



//Todo: Build Application
var app = builder.Build();

//Todo: Middleware Pipeline


app.UseRouting();

app.UseCors("AllowAll");

// app.UseAuthentication();
// app.UseAuthorization();

// Hangfire dashboard
app.UseHangfireDashboard("/hangfire");


//Todo: Endpoint Mapping


app.MapControllers();
app.MapRazorPages();

app.MapGet("/", () => "MediCore API is running");


//Todo: Database Migration + Seeding


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<AppDbContext>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    int maxRetries = 5;
    int delaySeconds = 5;

    for (int i = 1; i <= maxRetries; i++)
    {
        try
        {
            logger.LogInformation("Database migration attempt {Attempt} of {Max}", i, maxRetries);
            
            // Todo: Create EFMigrationsHistory table and all Identity tables
            await context.Database.MigrateAsync();
            
            //Todo Seed roles and admin
            var loggerSeed = services.GetRequiredService<ILoggerFactory>().CreateLogger("RoleSeeder");

            await RoleSeeder.SeedRolesAsync(roleManager, loggerSeed);

            // seed admin
            await DbInitializer.SeedAdminUser(services);
                        
            logger.LogInformation("Database migration and seeding completed successfully.");
            break; // Success! Exit the loop.
        }
        catch (Exception ex)
        {
            logger.LogWarning("Database not ready yet (Attempt {Attempt}): {Message}", i, ex.Message);
            
            if (i == maxRetries)
            {
                logger.LogCritical(ex, "Database migration failed after {Max} attempts. Application may not function.", maxRetries);
            }
            else
            {
                await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
            }
        }
    }
}


app.Run();


