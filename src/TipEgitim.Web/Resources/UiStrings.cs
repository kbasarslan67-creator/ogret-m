namespace TipEgitim.Web.Resources;

/// <summary>
/// Kullanıcıya dönük Türkçe metinler. Görünümler Türkçe metni yalnızca bu sınıf üzerinden okur.
/// Gerekçe: RESX kaynakları `dotnet build` içinde tipli erişim sınıfı üretmez (designer dosyası yalnızca
/// Visual Studio'da üretilir); elle yazılmış sabitler derleme zamanında denetlenir, araç zinciri gerektirmez
/// ve analizör uyarılarıyla çakışmaz. Yerelleştirme gerektiğinde bu sınıf IStringLocalizer ile değiştirilir.
/// </summary>
public static class UiStrings
{
    public const string AppName = "Tıp Eğitimi Platformu";
    public const string SkipToContent = "Ana içeriğe atla";
    public const string MainNavigationLabel = "Ana menü";

    public const string HomeTitle = "Ana Sayfa";
    public const string HomeLead = "Hoş geldiniz.";
    public const string HomeShellNotice = "Bu sürüm arayüz kabuğudur. Ekranlar sonraki dilimlerde eklenecektir.";

    public const string MenuHome = "Ana Sayfa";
    public const string MenuCurriculum = "Müfredat";
    public const string MenuAttendance = "Yoklama";
    public const string MenuBlueprint = "Belirtke";
    public const string MenuQuestionBank = "Soru Bankası";
    public const string MenuAdministration = "Yönetim";
}
