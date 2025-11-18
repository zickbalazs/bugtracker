using System.Globalization;
using CommunityToolkit.Maui.Converters;

namespace Bugtracker.Main.Converters;

public class StringToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        try
        {
            var colorCode = (string)value;

            if (colorCode == null)
                return Colors.Azure;

            return new Color(
                red: ConvertHexToInt(colorCode.Substring(1, 2)),
                blue: ConvertHexToInt(colorCode.Substring(3, 2)),
                green: ConvertHexToInt(colorCode.Substring(5, 2))
            );
        }
        catch
        {
            return Colors.Azure;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private int ConvertHexToInt(string hex) => System.Convert.ToInt32(hex, 16);
}