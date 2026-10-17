using TipEgitim.Application;
using TipEgitim.Infrastructure;
using TipEgitim.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Windows Hizmeti olarak çalışır; konsol uygulaması olarak çalıştırıldığında (ör. Linux) etkisizdir.
builder.Host.UseWindowsService(options => options.ServiceName = "TipEgitimPlatformu");

builder.Services.AddPlatformConfiguration(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddScoped<IDispatcher, Dispatcher>();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");
app.MapGet("/", () => "Tıp Eğitimi Platformu");

await app.RunAsync();
