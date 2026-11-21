using CommunityToolkit.Mvvm.ComponentModel;
using LifeManager.Services;

namespace LifeManager.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private double fontSize = Preferences.Default.Get("TaskFontSize", 16.0);

    private readonly string[] themes =
   {
    "White",
    "Beige",
    "Light Blue",
    "Dark",
    "Vampire",
    "Ubuntu Terminal"
};

    [ObservableProperty]
    private int selectedThemeIndex;

    public SettingsViewModel()
    {
        string savedTheme = Preferences.Default.Get("BackgroundColor", "White");

        int index = Array.IndexOf(themes, savedTheme);
        SelectedThemeIndex = index >= 0 ? index : 0;
    }

    partial void OnFontSizeChanged(double value)
    {
        Preferences.Default.Set("TaskFontSize", value);
    }

    partial void OnSelectedThemeIndexChanged(int value)
    {
        if (value < 0 || value >= themes.Length)
            return;

        string selectedTheme = themes[value];

        Preferences.Default.Set("BackgroundColor", selectedTheme);
        ThemeService.ApplyTheme(selectedTheme);
    }
}