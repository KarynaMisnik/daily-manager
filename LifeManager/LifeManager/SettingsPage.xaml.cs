using LifeManager.ViewModels;
using LifeManager.Services;

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
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
        FontSizeSlider.ValueChanged += (sender, e) =>
        {
            FontPreview.FontSize = e.NewValue;
        };

    }
}