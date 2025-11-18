using System.Globalization;
using Bugtracker.Main.Models;
using Bugtracker.Models;

namespace Bugtracker.Main.Converters;

public class BugToTitleConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var valueAsBug = (ObservableBug)value ?? new ObservableBug()
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