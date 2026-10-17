using TipEgitim.Web.Resources;

namespace TipEgitim.Web.Navigation;

/// <summary>Ana menünün tek tanımı. Düzen bu listeyi çizer; sayfalar menüye kendi bağlantılarını eklemez.</summary>
public static class MainMenu
{
    /// <summary>Menü girdileri. Bağlantılar Dilim 1 ile gelecek ekranlara işaret eder (şimdilik yer tutucu).</summary>
    public static IReadOnlyList<MenuDefinition> Items { get; } =
    [
        new(UiStrings.MenuHome, "/"),
        new(UiStrings.MenuCurriculum, "/mufredat"),
        new(UiStrings.MenuAttendance, "/yoklama"),
        new(UiStrings.MenuBlueprint, "/belirtke"),
        new(UiStrings.MenuQuestionBank, "/soru-bankasi"),
        new(UiStrings.MenuAdministration, "/yonetim"),
    ];

    /// <summary>Geçerli istek yolu için menü girdilerini, aktif olanı işaretleyerek döndürür.</summary>
    public static IReadOnlyList<MenuItem> ForPath(string currentPath)
    {
        ArgumentNullException.ThrowIfNull(currentPath);

        return Items
            .Select(item => new MenuItem(
                item.Label,
                item.Href,
                string.Equals(item.Href, currentPath, StringComparison.Ordinal)))
            .ToArray();
    }
}

/// <summary>Bir menü girdisinin tanımı.</summary>
public sealed record MenuDefinition(string Label, string Href);

/// <summary>Bir istek için çizilecek menü girdisi.</summary>
public sealed record MenuItem(string Label, string Href, bool IsCurrent);
