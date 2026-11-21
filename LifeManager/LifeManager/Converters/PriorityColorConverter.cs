using System.Globalization;

namespace LifeManager.Converters
{
    public class PriorityColorConverter : IValueConverter
    {
        public object Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            string resourceKey = value is int priority
                ? priority switch
                {
                    3 => "PriorityHigh",
                    2 => "PriorityMedium",
                    _ => "PriorityLow"
                }
                : "PriorityLow";

            return Application.Current!.Resources[resourceKey];
        }

        public object ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}