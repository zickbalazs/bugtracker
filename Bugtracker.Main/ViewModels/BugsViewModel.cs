using System.Collections.ObjectModel;
using Bugtracker.Main.Models;
using Bugtracker.Main.Statics;
using Bugtracker.Main.Views.Bugs;
using Bugtracker.Models;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Main.ViewModels;

public partial class BugsViewModel : ObservableObject
{
    private readonly IBugService _service;
    private readonly IUserService _userService;
    private readonly IPriorityService _priorityService;
    
    [ObservableProperty, 
     NotifyCanExecuteChangedFor(nameof(ViewBugCommand)), 
     NotifyCanExecuteChangedFor(nameof(EditBugCommand)),
     NotifyCanExecuteChangedFor(nameof(DeleteBugCommand))]
    private ObservableBug? selectedBug = null;

    [ObservableProperty,
     NotifyPropertyChangedFor(nameof(NoPriorities)),
    NotifyCanExecuteChangedFor(nameof(CreateBugCommand))]
    private int prioritiesCount = 0;
    
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
    
    [ObservableProperty,
     NotifyPropertyChangedFor(nameof(NoBugsAtAll))]
    private ObservableCollection<ObservableBug> bugs = [];

    [ObservableProperty, 
     NotifyPropertyChangedFor(nameof(NoBugsAtAll)),
     NotifyPropertyChangedFor(nameof(NoBugsAtAll)),
     NotifyPropertyChangedFor(nameof(NoPriorities)),
    NotifyCanExecuteChangedFor(nameof(CreateBugCommand))]
    private bool loadingFromDb = true;
    
    [ObservableProperty] 
    private int latestSelectedId = 0;
    

    public bool NoBugsAtAll => Bugs.Count == 0 && !LoadingFromDb && !NoPriorities;
    public bool NoPriorities => !LoadingFromDb && PrioritiesCount < 1;

    private bool ThereArePriorities() => !LoadingFromDb && PrioritiesCount > 0;
    
    
    private bool SelectedBugNotNull() => !LoadingFromDb && SelectedBug != null;
    private bool AllowedToEditSelected() => SelectedBugNotNull() && 
                                            (SelectedBug!.Author.Id == CurrentUser.Id || CurrentUser.IsAdmin) &&
                                            !SelectedBug.Solved; 
    public BugsViewModel(IBugService service, IUserService userService, IPriorityService priorityService)
    {
        _service = service;
        _userService = userService;
        _priorityService = priorityService;
        GetBugs();
    }

    public async Task GetBugs()
    {
        LoadingFromDb = true;
        Bugs = [..(await _service.GetAllAsync()).Select(ObservableBug.Parse).ToList()];
        CurrentUser = await _userService.GetUserByEmail(AuthData.GetEmail());
        PrioritiesCount = (await _priorityService.GetAllAsync()).Count;
        LoadingFromDb = false;
    }

    [RelayCommand(CanExecute = nameof(SelectedBugNotNull))]
    private async Task ViewBug()
    {
        await Shell.Current.GoToAsync($"bugdetails?id={SelectedBug!.Id}");
    }

    [RelayCommand(CanExecute = nameof(ThereArePriorities))]
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
        try
        {
            await _service.DeleteAsync(SelectedBug.Id);
            await GetBugs();
        }
        catch (Exception ex)
        {
            WeakReferenceMessenger.Default.Send(ex.Message);
        }
    }
}