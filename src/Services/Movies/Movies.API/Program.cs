using Microsoft.Extensions.DependencyInjection;
using Movies.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IMovieRepository, MovieRepository>();

//�[�JOAuth����
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options => {
        options.Authority = "https://localhost:5005";
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateAudience = false,
        };        
    });
builder.Services.AddAuthorization(options => {
    //�w�q���v�����AClaims�̭���Client_id�����n��MovieClient�~��ϥ�
    options.AddPolicy("ClientIdPolicy", policy=> policy.RequireClaim("client_id", "MovieClient"));
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
