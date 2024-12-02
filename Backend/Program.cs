using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("Jwt");
// var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);
var key = jwtSettings["Key"];
if (string.IsNullOrEmpty(key))
{
    throw new Exception("JWT key is missing in the configuration.");
}
var keyBytes = Encoding.ASCII.GetBytes(key);



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
    };
});

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Включаем использование CORS
app.UseCors("AllowAll");

// Включаем аутентификацию и авторизацию
app.UseAuthentication(); // Добавьте аутентификацию перед авторизацией
app.UseAuthorization();


app.UseHttpsRedirection();

// Включаем маршрутизацию для контроллеров
app.MapControllers();

app.Run();

