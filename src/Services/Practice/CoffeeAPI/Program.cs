using CoffeeAPI.Services;
using CoffeeDataAccess.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.Authority = "https://localhost:5443";
    });

builder.Services.AddAuthorization(options => {
    //定義授權策略，Claims裡面的Client_id必須要為MovieClient才能使用
    options.AddPolicy("APIScopePolicy", policy => policy.RequireClaim("scope", "CoffeeAPI.read"));
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("CoffeeAPI")));

builder.Services.AddScoped<ICoffeeShopService, CoffeeShopService>();

var app = builder.Build();

app.MapControllers();

app.Run();
