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
    }
}