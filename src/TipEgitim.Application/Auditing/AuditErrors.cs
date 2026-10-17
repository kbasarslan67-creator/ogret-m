using TipEgitim.SharedKernel;

namespace TipEgitim.Application.Auditing;

/// <summary>
/// Denetim modülünün hata kataloğu. Kodlar kararlıdır (noktalı, küçük harf); yeni kod buraya eklenir.
/// </summary>
public static class AuditErrors
{
    /// <summary>Eyleyen (actor) boş veya yalnızca boşluk.</summary>
    public static ResultError ActorRequired { get; } = new(
        "audit.actor_required",
        "Denetim kaydı için eyleyen belirtilmelidir.",
        ErrorType.Validation);

    /// <summary>İşlem adı boş veya yalnızca boşluk.</summary>
    public static ResultError ActionRequired { get; } = new(
        "audit.action_required",
        "Denetim kaydı için işlem adı belirtilmelidir.",
        ErrorType.Validation);

    /// <summary>Denetim kaydı depolamaya yazılamadı.</summary>
    public static ResultError StorageFailed { get; } = new(
        "audit.storage_failed",
        "Denetim kaydı kaydedilemedi.",
        ErrorType.Unexpected);

    /// <summary>Hedef türü boş veya yalnızca boşluk.</summary>
    public static ResultError TargetTypeRequired { get; } = new(
        "audit.target_type_required",
        "Denetim kaydı için hedef türü belirtilmelidir.",
        ErrorType.Validation);

    /// <summary>Hedef kimliği boş veya yalnızca boşluk.</summary>
    public static ResultError TargetIdRequired { get; } = new(
        "audit.target_id_required",
        "Denetim kaydı için hedef kimliği belirtilmelidir.",
        ErrorType.Validation);
}
