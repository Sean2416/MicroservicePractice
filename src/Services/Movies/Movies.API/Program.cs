using Microsoft.Extensions.DependencyInjection;
using Movies.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IMovieRepository, MovieRepository>();

//加入OAuth驗證
builder.Services.AddAuthentication("Bearer")
.AddJwtBearer("Bearer", options => {
    options.Authority = "https://localhost:5006";
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = false,
    };        
});

builder.Services.AddAuthorization(options => {
    //定義授權策略，Claims裡面的Client_id必須要為MovieClient才能使用
    options.AddPolicy("APIScopePolicy", policy => policy.RequireClaim("scope", "MovieAPI"));
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
