using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
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
builder.Services.AddRazorPages();

var app = builder.Build();

// Kullanıcıya dönük sıralama ve biçimlendirme kültürü: ADR-003 gereği tr-TR (Platform:DefaultCulture).
var culture = app.Services.GetRequiredService<IOptions<PlatformOptions>>().Value.DefaultCulture;
var supportedCultures = new[] { new CultureInfo(culture) };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culture),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
});

app.UseStaticFiles();
app.MapRazorPages();
app.MapHealthChecks("/health");

await app.RunAsync();

/// <summary>Test projesinin <c>WebApplicationFactory</c> ile uygulamaya erişebilmesi için.</summary>
public partial class Program
{
}
