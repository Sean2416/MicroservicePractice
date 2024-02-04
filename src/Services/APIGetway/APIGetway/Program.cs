using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", false, true);

builder.Services.AddAuthentication()
    .AddJwtBearer("TestKey", options =>
    {
        options.Authority = "https://localhost:5006";
        options.TokenValidationParameters.ValidateAudience = false;
    });

builder.Services.AddOcelot(builder.Configuration);


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseRouting();

app.UseEndpoints(endpoints => endpoints.MapControllers());

await app.UseOcelot();

app.Run();
