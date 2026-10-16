
namespace TipEgitim.SharedKernel.Tests;

public sealed class ResultTests
{
    private static readonly ResultError _sampleError = new("sample.error", "Örnek hata", ErrorType.Validation);

    [Fact]
    public void SuccessHasNoError()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Null(result.Error);
    }

    [Fact]
    public void FailureCarriesError()
    {
        var result = Result.Failure(_sampleError);

        Assert.True(result.IsFailure);
        Assert.Equal(_sampleError, result.Error);
    }

    [Fact]
    public void SuccessWithValueExposesValue()
    {
        var result = Result.Success(42);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void FailureValueAccessThrows()
    {
        var result = Result.Failure<int>(_sampleError);

        Assert.True(result.IsFailure);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public void FailureRejectsNullError()
    {
        Assert.Throws<ArgumentNullException>(() => Result.Failure(null!));
    }
}
