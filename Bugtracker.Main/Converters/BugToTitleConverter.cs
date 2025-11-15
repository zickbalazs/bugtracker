using System.Globalization;
using Bugtracker.Models;

namespace Bugtracker.Main.Converters;

public class BugToTitleConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var valueAsBug = (Bug)value ?? new Bug
        {
            Id = -1,
            Title = string.Empty,
            Description = string.Empty,
            ShortDescription = string.Empty
        };
        return $"#{valueAsBug.Id} | {valueAsBug.Title}{(valueAsBug.Solved ? " (solved)" :"")}";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}