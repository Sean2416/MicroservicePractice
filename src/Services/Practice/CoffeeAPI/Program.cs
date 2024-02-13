using CoffeeAPI.Services;
using CoffeeDataAccess.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("CoffeeAPI")));

builder.Services.AddScoped<ICoffeeShopService, CoffeeShopService>();

var app = builder.Build();

app.MapControllers();

app.Run();
