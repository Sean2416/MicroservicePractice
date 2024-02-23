using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.Values;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", false, true);

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

builder.Services.AddAuthentication()
    .AddJwtBearer("TestKey", options =>
    {
        options.Authority = "https://localhost:5443";
        options.TokenValidationParameters.ValidateAudience = false;
    });

builder.Services.AddOcelot(builder.Configuration);


var app = builder.Build();

app.UseCors("AllowAllOrigins");

app.MapGet("/", () => "Hello World!");

app.UseRouting();

app.UseEndpoints(endpoints => endpoints.MapControllers());

await app.UseOcelot();

app.Run();
