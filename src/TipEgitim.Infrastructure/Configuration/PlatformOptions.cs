namespace TipEgitim.Infrastructure.Configuration;

/// <summary>Platform yapılandırma bölümü ("Platform" anahtarı).</summary>
public sealed class PlatformOptions
{
    /// <summary>Bağlanılacak yapılandırma bölümünün adı.</summary>
    public const string SectionName = "Platform";

    /// <summary>Kurum adı. Zorunludur.</summary>
    public string InstitutionName { get; set; } = string.Empty;

    /// <summary>Veritabanı bağlantı dizesinin adı. Zorunludur.</summary>
    public string DatabaseConnectionName { get; set; } = string.Empty;

    /// <summary>Kullanıcıya dönük sıralama ve biçimlendirme kültürü.</summary>
    public string DefaultCulture { get; set; } = "tr-TR";
}
