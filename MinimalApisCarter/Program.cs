using Carter;
using MinimalApisCarter.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Dependencias
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseInMemoryDatabase(nameof(MyDbContext)));
builder.Services.AddCarter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Minimal APIs & Carter"
    });
});

var app = builder.Build();

// Middlewares
app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
    c.RoutePrefix = "api";
});
app.MapCarter();

app.Run();