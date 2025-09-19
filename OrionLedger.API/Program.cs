using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using OrionLedger.Infrastructure.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Configuração de Serviços
// ----------------------

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext PostgreSQL
builder.Services.AddDbContext<OrionLedgerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = $"{builder.Configuration["Redis:Host"]}:{builder.Configuration["Redis:Port"]}";
    options.InstanceName = "OrionLedger:";
});

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
.AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// **Adicionando autorização**
builder.Services.AddAuthorization(); // <- ESSENCIAL para usar app.UseAuthorization()

// Health Checks
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection"), name: "PostgreSQL")
    .AddRedis($"{builder.Configuration["Redis:Host"]}:{builder.Configuration["Redis:Port"]}", name: "Redis");

var app = builder.Build();

// ----------------------
// Pipeline HTTP
// ----------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// **Autenticação e Autorização**
app.UseAuthentication();
app.UseAuthorization();

// ----------------------
// Endpoints
// ----------------------

// Health check
app.MapHealthChecks("/health");

// Exemplo: WeatherForecast
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

// ----------------------
// Model Record
// ----------------------
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
