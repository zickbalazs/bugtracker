using Bugtracker.Main.ViewModels;
using Bugtracker.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bugtracker.Main.Models;

public partial class ObservableBug : ObservableObject
{
    [ObservableProperty] 
    private int id;

    [ObservableProperty] 
    private string title;

    [ObservableProperty] 
    private string shortDescription;

    [ObservableProperty] 
    private string description;

    [ObservableProperty] 
    private string? associatedFileName;

    [ObservableProperty] 
    private bool solved;

    [ObservableProperty] 
    private DateTime created;

    [ObservableProperty]
    private DateTime? solvedOn;

    [ObservableProperty] 
    private Priority priority;

    [ObservableProperty]
    private IList<BugComment> comments = [];

    [ObservableProperty] 
    private User author;

    public static ObservableBug Parse(Bug bug) => new()
    {
        Id = bug.Id,
        Title = bug.Title,
        ShortDescription = bug.ShortDescription,
        Description = bug.Description,
        AssociatedFileName = bug.AssociatedFileName,
        Solved = bug.Solved,
        SolvedOn = bug.SolvedOn,
        Created = bug.Created,
        Author = bug.Author,
        Priority = bug.Priority,
        Comments = bug.Comments
    };
}