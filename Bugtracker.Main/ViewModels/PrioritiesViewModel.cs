using System.Collections.ObjectModel;
using Bugtracker.Main.Models;
using Bugtracker.Main.Statics;
using Bugtracker.Models;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Main.ViewModels;

public partial class PrioritiesViewModel : ObservableObject
{
    private readonly IPriorityService _priorityService;
    private readonly IUserService _userService;
    
    [ObservableProperty,
    NotifyCanExecuteChangedFor(nameof(DeleteCommand)),
    NotifyCanExecuteChangedFor(nameof(EditCommand))]
    private ObservablePriority? selectedPriority;

    [ObservableProperty,
    NotifyCanExecuteChangedFor(nameof(EditCommand))]
    private User currentUser = new()
    {
        
    };

    [ObservableProperty,
    NotifyCanExecuteChangedFor(nameof(EditCommand)),
    NotifyCanExecuteChangedFor(nameof(DeleteCommand))] 
    private bool isLoading = true;
    
    [ObservableProperty] 
    private ObservableCollection<ObservablePriority> priorities = [];


    public PrioritiesViewModel(IPriorityService priorityService, IUserService userService)
    {
        _priorityService = priorityService;
        _userService = userService;
        Connectivity.Current.ConnectivityChanged += ConnectivityAvailable; 
        GetPriorities();
    }

    public async Task GetPriorities()
    {
        IsLoading = true;

        var dbData = (await _priorityService.GetAllAsync()).Select(ObservablePriority.Parse);
        
        Priorities = [..dbData];
        CurrentUser = await _userService.GetUserByEmail(AuthData.GetEmail());
        IsLoading = false;
    }

    [ObservableProperty]
    private bool internetAvailable = Connectivity.NetworkAccess == NetworkAccess.Internet;
    
    public void ConnectivityAvailable(object? sender, ConnectivityChangedEventArgs args)
    {
        InternetAvailable = args.NetworkAccess == NetworkAccess.Internet;        
        AddPriorityCommand.NotifyCanExecuteChanged();
    }

    private bool DoesntHavePosts() => !IsLoading && SelectedPriority is { Bugs.Count: < 1 };

    private bool CanEdit() => !IsLoading && SelectedPriority != null && CurrentUser.IsAdmin;
    
    [RelayCommand(CanExecute = nameof(InternetAvailable))]
    private async Task AddPriority()
    {
        await Shell.Current.GoToAsync("createPriority");
    }

    [RelayCommand(CanExecute = nameof(DoesntHavePosts))]
    private async Task Delete()
    {
        try
        {
            await _priorityService.DeleteAsync(SelectedPriority!.Id);
            await GetPriorities();
        }
        catch (Exception ex)
        {
            WeakReferenceMessenger.Default.Send(ex.Message);
        }
    }

    [RelayCommand(CanExecute = nameof(CanEdit))]
    public async Task Edit()
    {
        await Shell.Current.GoToAsync($"editPriority?id={SelectedPriority.Id}");
    }
}