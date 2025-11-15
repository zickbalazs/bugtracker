using System.Collections.ObjectModel;
using Bugtracker.Main.Statics;
using Bugtracker.Models;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bugtracker.Main.ViewModels;

public partial class BugsViewModel : ObservableObject
{
    private readonly IBugService _service;
    private readonly IUserService _userService;
    
    [ObservableProperty, 
     NotifyCanExecuteChangedFor(nameof(ViewBugCommand)), 
     NotifyCanExecuteChangedFor(nameof(EditBugCommand)),
     NotifyCanExecuteChangedFor(nameof(DeleteBugCommand))]
    private Bug? selectedBug = null;

    [ObservableProperty, 
     NotifyCanExecuteChangedFor(nameof(EditBugCommand)), 
     NotifyCanExecuteChangedFor(nameof(DeleteBugCommand))]
    private User currentUser = new User()
    {
        Name = "",
        Email = "",
        Id = -1,
        IsAdmin = false
    };
    
    [ObservableProperty, NotifyPropertyChangedFor(nameof(NoBugsAtAll))] private List<Bug> bugs = [];

    [ObservableProperty, NotifyPropertyChangedFor(nameof(NoBugsAtAll))] private bool loadingFromDb = true;
    [ObservableProperty] private int latestSelectedId = 0;

    public bool NoBugsAtAll => Bugs.Count == 0 && !LoadingFromDb;
    private bool SelectedBugNotNull() => !LoadingFromDb && SelectedBug != null;
    private bool AllowedToEditSelected() => SelectedBugNotNull() && 
                                            (SelectedBug!.Author.Id == CurrentUser.Id || CurrentUser.IsAdmin) &&
                                            !SelectedBug.Solved; 
    
    public BugsViewModel(IBugService service, IUserService userService)
    {
        _service = service;
        _userService = userService;
        GetBugs();
    }

    public async Task GetBugs()
    {
        LoadingFromDb = true;
        Bugs = await _service.GetAllAsync();
        CurrentUser = await _userService.GetUserByEmail(AuthData.GetEmail());
        LoadingFromDb = false;
    }

    [RelayCommand(CanExecute = nameof(SelectedBugNotNull))]
    private async Task ViewBug()
    {
        await Shell.Current.GoToAsync($"bugdetails?id={SelectedBug!.Id}");
    }

    [RelayCommand]
    private async Task CreateBug()
    {
        await Shell.Current.GoToAsync("createBug");
    }

    [RelayCommand(CanExecute = nameof(AllowedToEditSelected))]
    private async Task EditBug()
    {
        await Shell.Current.GoToAsync($"editBug?id={SelectedBug!.Id}");
    }

    [RelayCommand(CanExecute = nameof(AllowedToEditSelected))]
    private async Task DeleteBug()
    {
        await _service.DeleteAsync(SelectedBug.Id);
        await GetBugs();
    }
}