using System.Collections.ObjectModel;
using Bugtracker.Models;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bugtracker.Main.ViewModels;

public partial class BugsViewModel : ObservableObject
{
    private readonly IBugService _service;
    
    [ObservableProperty, NotifyPropertyChangedFor(nameof(NoBugsAtAll))] private List<Bug> bugs = [];

    [ObservableProperty, NotifyPropertyChangedFor(nameof(HasSolved))]
    private List<Bug> solvedBugs = [];
    
    [ObservableProperty, NotifyPropertyChangedFor(nameof(HasUnsolved))]
    private List<Bug> unsolvedBugs = [];
    
    [ObservableProperty, NotifyPropertyChangedFor(nameof(NoBugsAtAll))] private bool loadingFromDb = true;
    [ObservableProperty] private int latestSelectedId = 0;

    public bool NoBugsAtAll => Bugs.Count == 0 && !LoadingFromDb;
    public bool HasUnsolved => UnsolvedBugs.Count > 0;
    public bool HasSolved => SolvedBugs.Count > 0;
    
    public BugsViewModel(IBugService service)
    {
        _service = service;
        GetBugs();
    }

    public async Task GetBugs()
    {
        Bugs = await _service.GetAllAsync();
        UnsolvedBugs = [..Bugs.Where(x => !x.Solved)];
        SolvedBugs = [..Bugs.Where(x => x.Solved)];
        LoadingFromDb = false;
    }

    [RelayCommand]
    private async Task ViewBug(int id)
    {
        LatestSelectedId = id;
        await Shell.Current.GoToAsync($"bugdetails?id={id}");
    }

    [RelayCommand]
    private async Task CreateBug()
    {
        await Shell.Current.GoToAsync("createBug");
    }
}