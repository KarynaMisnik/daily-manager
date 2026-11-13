namespace LifeManager;

[QueryProperty(nameof(FontSize), "fontSize")]

public partial class SettingsPage : ContentPage
{
    public string FontSize
    {
        set
        {
            if (double.TryParse(value, out double size))
            {
                FontSizeSlider.Value = size;
            }
        }
    }
    public SettingsPage()
	{
		InitializeComponent();
        FontSizeSlider.ValueChanged += (sender, e) =>
        {
            Preferences.Default.Set("TaskFontSize", e.NewValue);
            FontPreview.FontSize = e.NewValue;
        };

        BackgroundPicker.SelectedIndexChanged += (sender, e) =>
        {
            if (BackgroundPicker.SelectedIndex >= 0)
            {
                string selectedColor = BackgroundPicker.Items[
                    BackgroundPicker.SelectedIndex
                ];

                Preferences.Default.Set("BackgroundColor", selectedColor);

                ThemePreview.BackgroundColor = selectedColor switch
                {
                    "Beige" => Color.FromArgb("#F5F5DC"),
                    "Light Blue" => Color.FromArgb("#E3F2FD"),
                    "Dark" => Color.FromArgb("#17191E"),
                    "Vampire" => Color.FromArgb("#280D19"),
                    _ => Colors.White
                };

                ThemePreviewText.TextColor = selectedColor switch
                {
                    "Dark" => Colors.White,
                    "Vampire" => Color.FromArgb("#FFB4C8"),
                    _ => Colors.Black
                };
            }
        };

        string savedColor = Preferences.Default.Get("BackgroundColor", "White");

        int colorIndex = BackgroundPicker.Items.IndexOf(savedColor);

        BackgroundPicker.SelectedIndex = colorIndex >= 0 ? colorIndex : 0;
    }
}