using System.ComponentModel.DataAnnotations;
using Bugtracker.Main.Models;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Main.ViewModels;

public partial class EditPriorityViewModel(IPriorityService service) : ObservableValidator
{
    [ObservableProperty,
    NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    private bool isLoading = false;
    
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
    
    

    private bool CanSubmit()
    {
        ValidateAllProperties();
        return !IsLoading && !HasErrors;
    }

    private PriorityDto dto => new()
    {
        Title = Title,
        ColorCode = ColorCode
    };
    
    public async Task GetPriority(int id)
    {
        IsLoading = true;
        var priority = await service.GetAsync(id);
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
            await service.ModifyAsync(Id, dto);
            await Shell.Current.GoToAsync("../");
        }
        catch (Exception ex)
        {
            WeakReferenceMessenger.Default.Send(ex.Message);
        }
    }
}