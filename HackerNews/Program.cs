using HackerNews.API.Middleware;
using HackerNews.Domain.Abstract;
using HackerNews.Domain.Interface;
using Microsoft.ApplicationInsights;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("cors", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
        builder.WithHeaders("Content-Type");
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IHttpClientFactoryService, HttpClientFactoryService>();
builder.Services.AddScoped<IHackerNewsService, HackerNewsService>();
builder.Services.AddTransient<TelemetryClient, TelemetryClient>();

builder.Services.AddControllers();
        
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("cors");
app.UseMiddleware<RequestResponseExceptionLogging>();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
