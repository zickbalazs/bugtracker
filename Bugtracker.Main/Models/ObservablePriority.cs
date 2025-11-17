using System.Collections.ObjectModel;
using Bugtracker.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bugtracker.Main.Models;

public partial class ObservablePriority : ObservableObject
{
    [ObservableProperty]
    private int id;
    
    [ObservableProperty]
    private string title;
    
    [ObservableProperty]
    private string colorCode;

    [ObservableProperty]
    private ObservableCollection<ObservableBug> bugs;

    public static ObservablePriority Parse(Priority priority) => new()
    {
        Id = priority.Id,
        Title = priority.Title,
        ColorCode = priority.ColorCode,
        Bugs = [..priority.Bugs.Select(ObservableBug.Parse)]
    };
}