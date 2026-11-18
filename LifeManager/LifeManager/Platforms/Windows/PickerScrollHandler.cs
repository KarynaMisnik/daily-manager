using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Input;

namespace LifeManager.Platforms.Windows;

public static class PickerScrollHandler
{
    public static void Configure()
    {
        PickerHandler.Mapper.AppendToMapping("DisableMouseWheel", (handler, view) => // lets us customize MAUI's native Windows Picker.
        {
            if (handler.PlatformView is Microsoft.UI.Xaml.Controls.ComboBox comboBox)
            {
                comboBox.PointerWheelChanged += OnPointerWheelChanged; // detects mouse-wheel movement
            }
        });
    }

    private static void OnPointerWheelChanged(
        object sender,
        PointerRoutedEventArgs e)
    {
        if (sender is Microsoft.UI.Xaml.Controls.ComboBox comboBox
            && !comboBox.IsDropDownOpen)
        {
            e.Handled = true; // prevents further processing of that wheel event when the dropdown is closed
        }
    }
}