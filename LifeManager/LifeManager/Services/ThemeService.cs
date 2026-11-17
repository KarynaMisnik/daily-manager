namespace LifeManager.Services;

public static class ThemeService // static - no need for object
{
    public static void ApplyTheme(string themeName)
    {

        var resources = Application.Current?.Resources; // accesses the global resources defined in App.xaml and its merged dictionaries

        if (resources == null)
            return;

        if (themeName == "Ubuntu Terminal")
        {
            resources["ThemeBackground"] = Color.FromArgb("#300A24");
            resources["ThemeSurface"] = Color.FromArgb("#24101F");
            resources["ThemeText"] = Color.FromArgb("#EEEEEC");
            resources["ThemeSecondaryText"] = Color.FromArgb("#AEA79F");
            resources["ThemeAccent"] = Color.FromArgb("#E95420");
            resources["ThemeBorder"] = Color.FromArgb("#704A60");
        }

        else if (themeName == "Beige")
        {
            resources["ThemeBackground"] = Color.FromArgb("#F5F0E6");
            resources["ThemeSurface"] = Color.FromArgb("#FFF9F0");
            resources["ThemeText"] = Color.FromArgb("#40362D");
            resources["ThemeSecondaryText"] = Color.FromArgb("#766858");
            resources["ThemeAccent"] = Color.FromArgb("#A47750");
            resources["ThemeBorder"] = Color.FromArgb("#D9CBB8");
        }
        else if (themeName == "Light Blue")
        {
            resources["ThemeBackground"] = Color.FromArgb("#EAF4FF");
            resources["ThemeSurface"] = Color.FromArgb("#FFFFFF");
            resources["ThemeText"] = Color.FromArgb("#203B57");
            resources["ThemeSecondaryText"] = Color.FromArgb("#59738C");
            resources["ThemeAccent"] = Color.FromArgb("#3979B8");
            resources["ThemeBorder"] = Color.FromArgb("#C6DFF5");
        }
        else if (themeName == "Dark")
        {
            resources["ThemeBackground"] = Color.FromArgb("#17191E");
            resources["ThemeSurface"] = Color.FromArgb("#252830");
            resources["ThemeText"] = Color.FromArgb("#F0F0F0");
            resources["ThemeSecondaryText"] = Color.FromArgb("#B0B5C0");
            resources["ThemeAccent"] = Color.FromArgb("#7C9CF5");
            resources["ThemeBorder"] = Color.FromArgb("#414652");
        }
        else if (themeName == "Vampire")
        {
            resources["ThemeBackground"] = Color.FromArgb("#280D19");
            resources["ThemeSurface"] = Color.FromArgb("#3B1425");
            resources["ThemeText"] = Color.FromArgb("#FFE4EC");
            resources["ThemeSecondaryText"] = Color.FromArgb("#D6A1B2");
            resources["ThemeAccent"] = Color.FromArgb("#C83F67");
            resources["ThemeBorder"] = Color.FromArgb("#704052");
        }

        else
        {
            // Default White theme
            resources["ThemeBackground"] = Color.FromArgb("#FFFFFF");
            resources["ThemeSurface"] = Color.FromArgb("#F5F5F5");
            resources["ThemeText"] = Color.FromArgb("#202124");
            resources["ThemeSecondaryText"] = Color.FromArgb("#666666");
            resources["ThemeAccent"] = Color.FromArgb("#2563EB");
            resources["ThemeBorder"] = Color.FromArgb("#DDDDDD");
        }
    }
}