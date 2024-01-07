using Discount.API.Repositories;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods;
using VaultSharp;
using Discount.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDiscountRepository, MsDiscountRepository>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton((serviceProvider) =>
{
    IAuthMethodInfo authMethod = new TokenAuthMethodInfo(vaultToken: "root");

    VaultClientSettings vaultClientSettings = new VaultClientSettings("http://127.0.0.1:8200", authMethod);
    IVaultClient vaultClient = new VaultClient(vaultClientSettings);
    return vaultClient;
});

builder.Services.AddSingleton<VaultExtensions>();

var app = builder.Build();

//app.MigrateDatabase<Program>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
