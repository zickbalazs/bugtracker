using System.ComponentModel.DataAnnotations;
using Bugtracker.Main.Models;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Main.ViewModels;

public partial class EditPriorityViewModel : ObservableValidator
{
    [ObservableProperty,
    NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    private bool isLoading = false;


    [ObservableProperty, 
     NotifyCanExecuteChangedFor(nameof(SubmitCommand))] 
    private bool internetIsAvailable = Connectivity.NetworkAccess == NetworkAccess.Internet;
    
    [ObservableProperty]
    private int id = -1;
    
    
    [ObservableProperty] 
    private string pageTitle = "Loading...";

    [ObservableProperty,
     NotifyCanExecuteChangedFor(nameof(SubmitCommand)),
     Required]
    private string title = string.Empty;
    [ObservableProperty, 
     NotifyCanExecuteChangedFor(nameof(SubmitCommand)),
     Required,
     RegularExpression(@"^#?([0-9a-f]{6}|[0-9a-f]{3})$")]
    private string colorCode = string.Empty;

    private readonly IPriorityService _service;

    public EditPriorityViewModel(IPriorityService service)
    {
        _service = service;
        Connectivity.ConnectivityChanged +=
            (_, args) => InternetIsAvailable = args.NetworkAccess == NetworkAccess.Internet;
    }


    private bool CanSubmit()
    {
        ValidateAllProperties();
        return !IsLoading && !HasErrors && InternetIsAvailable;
    }

    private PriorityDto dto => new()
    {
        Title = Title,
        ColorCode = ColorCode
    };
    
    public async Task GetPriority(int id)
    {
        IsLoading = true;
        var priority = await _service.GetAsync(id);
        Id = priority.Id;
        Title = priority.Title;
        ColorCode = priority.ColorCode;
        PageTitle = $"Editing {Title}";
        IsLoading = false;
    }

    [RelayCommand(CanExecute = nameof(CanSubmit))]
    private async Task Submit()
    {
        try
        {
            await _service.ModifyAsync(Id, dto);
            await Shell.Current.GoToAsync("../");
        }
        catch (Exception ex)
        {
            WeakReferenceMessenger.Default.Send(ex.Message);
        }
    }
}