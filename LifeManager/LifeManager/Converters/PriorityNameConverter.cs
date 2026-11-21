using System.Globalization;

namespace LifeManager.Converters
{
    public class PriorityNameConverter : IValueConverter
    {
        public object Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            return value is int priority
                ? priority switch
                {
                    3 => "HIGH",
                    2 => "MEDIUM",
                    _ => "LOW"
                }
                : "LOW";
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