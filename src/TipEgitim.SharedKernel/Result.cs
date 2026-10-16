namespace TipEgitim.SharedKernel;

/// <summary>Değer döndürmeyen bir işlemin sonucu. Beklenen hatalar için istisna yerine kullanılır.</summary>
public class Result
{
    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess == (error is not null))
        {
            throw new ArgumentException("Başarılı sonuç hata taşıyamaz; başarısız sonuç hata taşımak zorundadır.", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error? Error { get; }

    public static Result Success() => new(true, null);

    public static Result Failure(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new Result(false, error);
    }

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, null);

    public static Result<TValue> Failure<TValue>(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new Result<TValue>(default, false, error);
    }
}

/// <summary>Değer döndüren bir işlemin sonucu.</summary>
/// <typeparam name="TValue">Başarı durumunda taşınan değer.</typeparam>
public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    internal Result(TValue? value, bool isSuccess, Error? error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>Yalnızca başarılı sonuçta okunabilir; başarısızda istisna fırlatır (programlama hatası).</summary>
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Başarısız bir sonucun değeri okunamaz.");
}
