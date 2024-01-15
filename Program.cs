using weather_forecast_api;
using weather_forecast_api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{

    options.AddDefaultPolicy(
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IOptions, Options>();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();







