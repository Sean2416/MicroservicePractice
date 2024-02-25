using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewServer.Data;
using NewServer;
using IdentityServer4.Configuration;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly.GetName().Name;
var defaultConnString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder
                .AllowCredentials()
                .WithOrigins("https://localhost:44357")
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


builder.Services.AddDbContext<AspNetIdentityDbContext>(options =>
    options.UseSqlServer(defaultConnString,
        b => b.MigrationsAssembly(assembly)));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AspNetIdentityDbContext>();

builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;

    options.UserInteraction = new UserInteractionOptions
    {
        LogoutUrl = "/Account/Logout",
        LoginUrl = "/Account/Login",
        LoginReturnUrlParameter = "returnUrl"
    };
})
    .AddAspNetIdentity<ApplicationUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b =>
        b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
        b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
    })
    .AddDeveloperSigningCredential();

builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        //預設的External login方法會從這個Scheme取資料
        googleOptions.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        googleOptions.ClientId = "474316682203-bm7eh846qfm6kc2qs1e2uqm76bjso97b.apps.googleusercontent.com";
        googleOptions.ClientSecret = "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";
    })
.AddLine(options =>
{
     options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
        options.Scope.Add("real_name");
        options.Scope.Add("gender");
        options.Scope.Add("birthdate");
        options.Scope.Add("address");
        options.Scope.Add("phone");
        options.Scope.Add("email");
        options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        options.ClientId = "2003601821";
        options.ClientSecret = "f67d1c3871877619e842cb20c8f2db3e";
    });

builder.Services.AddControllersWithViews();


var app = builder.Build();
app.UseCors("AllowAllOrigins");
app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();

