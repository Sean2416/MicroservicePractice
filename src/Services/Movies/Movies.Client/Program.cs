using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Net.Http.Headers;
using Movies.Client.HttpHandlers;
using Movies.Client.Services;

var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Events.OnSigningOut = async e => { await e.HttpContext.RevokeUserRefreshTokenAsync(); };
    })
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        // Note: these settings must match the application details, and preferably stored as secrets/configuration
        options.Authority = builder.Configuration.GetConnectionString("IdentityServer") ?? throw new Exception("IdentityServer connection string is missing");
        options.ClientId = "movies_mvc_client";
        options.ClientSecret = "ClientSecret1";
        options.ResponseType = "code";

        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
       // options.Scope.Add("roles");
       // options.Scope.Add("offline_access"); // refresh token

        options.Scope.Add("MovieAPI");

        //options.ClaimActions.MapJsonKey("role", "role"); // If below doesnt work, try this
        options.ClaimActions.MapUniqueJsonKey("role", "role");


        options.SaveTokens = true;

        //再取得Id_Token後，是否要直接至User Info Endpoint取得User Info
        options.GetClaimsFromUserInfoEndpoint = true;

        options.TokenValidationParameters = new()
        {
            NameClaimType = JwtClaimTypes.GivenName,
            RoleClaimType = JwtClaimTypes.Role,
            ValidateLifetime = true,
            RequireExpirationTime = true,
        };
    });

    builder.Services.AddAccessTokenManagement().ConfigureBackchannelHttpClient(); // TODO: add polly resilience policies for refresh token
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<IMovieApiService, MovieApiService>();
    builder.Services.AddTransient<AuthenticationDelegatingHandler>();

    builder.Services.AddHttpClient("MovieAPIClient", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration.GetConnectionString("MoviesApiGw") ?? throw new Exception("MoviesApi connection string is missing"));
        client.DefaultRequestHeaders.Clear();
        //client.DefaultRequestHeaders.Accept.Add(new("application/json"));
        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
    })
       .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

    builder.Services.AddHttpClient("IDPClient", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration.GetConnectionString("IdentityServer") ?? throw new Exception("IdentityServer connection string is missing"));
        client.DefaultRequestHeaders.Clear();
        //client.DefaultRequestHeaders.Accept.Add(new("application/json"));
        client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
    });

    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.AccessDeniedPath = $"/Account/AccessDenied";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
