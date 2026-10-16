namespace TipEgitim.SharedKernel;

/// <summary>Beklenen bir iş hatası. Kod makine içindir, mesaj kullanıcıya gösterilebilir.</summary>
/// <param name="Code">Kararlı, noktalı hata kodu (örn. "curriculum.year_frozen").</param>
/// <param name="Message">Kullanıcıya gösterilebilir açıklama.</param>
/// <param name="Type">Hata sınıfı.</param>
public sealed record Error(string Code, string Message, ErrorType Type);
