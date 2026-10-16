using TipEgitim.Application;
using TipEgitim.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();
builder.Services.AddScoped<IDispatcher, Dispatcher>();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");
app.MapGet("/", () => "Tıp Eğitimi Platformu");

await app.RunAsync();
