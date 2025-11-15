using System.Globalization;

namespace Bugtracker.Main.Converters;

public class SolvedDateToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var valueAsDateTime = (DateTime?)value;

        
        
        return $"This bug has been marked as solved at: {valueAsDateTime ?? DateTime.Now}";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}