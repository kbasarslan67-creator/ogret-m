using Microsoft.Extensions.Options;

namespace TipEgitim.Infrastructure.Configuration;

/// <summary>Platform yapılandırmasının zorunlu alanlarını doğrular.</summary>
public sealed class PlatformOptionsValidator : IValidateOptions<PlatformOptions>
{
    /// <inheritdoc />
    public ValidateOptionsResult Validate(string? name, PlatformOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var failures = new List<string>();

        if (string.IsNullOrWhiteSpace(options.InstitutionName))
        {
            failures.Add("Platform:InstitutionName zorunludur ve boş olamaz.");
        }

        if (string.IsNullOrWhiteSpace(options.DatabaseConnectionName))
        {
            failures.Add("Platform:DatabaseConnectionName zorunludur ve boş olamaz.");
        }

        return failures.Count == 0
            ? ValidateOptionsResult.Success
            : ValidateOptionsResult.Fail(failures);
    }
}
