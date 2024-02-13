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
    //�w�q���v�����AClaims�̭���Client_id�����n��MovieClient�~��ϥ�
    options.AddPolicy("APIScopePolicy", policy => policy.RequireClaim("scope", "CoffeeAPI.read"));
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("CoffeeAPI")));

builder.Services.AddScoped<ICoffeeShopService, CoffeeShopService>();

var app = builder.Build();

app.MapControllers();

app.Run();
