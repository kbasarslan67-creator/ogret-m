namespace TipEgitim.SharedKernel;

/// <summary>Beklenen hata sınıfları. HTTP eşlemesi Web katmanında yapılır.</summary>
public enum ErrorType
{
    Validation,
    NotFound,
    Conflict,
    Forbidden,
    Unexpected,
}
