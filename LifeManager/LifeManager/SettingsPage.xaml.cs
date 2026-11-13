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
            }
        };

        string savedColor = Preferences.Default.Get("BackgroundColor", "White");

        int colorIndex = BackgroundPicker.Items.IndexOf(savedColor);

        BackgroundPicker.SelectedIndex = colorIndex >= 0 ? colorIndex : 0;
    }
}