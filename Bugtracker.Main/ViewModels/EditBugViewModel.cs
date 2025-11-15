using System.ComponentModel.DataAnnotations;
using Bugtracker.Models;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Main.ViewModels;

public partial class EditBugViewModel(
    IBugService bugService,
    IPriorityService priorityService) : ObservableValidator
{
    [ObservableProperty] 
    private int id;

    [ObservableProperty] 
    private IList<Priority> priorities = [];

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    [Required]
    [MinLength(3)]
    private string title = string.Empty;
    
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    [Required]
    [MinLength(8)]
    private string shortDesc = string.Empty;
    
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    [Required]
    private string description = string.Empty;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
    [Required]
    private Priority? selectedPriority = null;

    private BugDto dto => new BugDto
    {
        Title = Title,
        Description = Description,
        ShortDescription = ShortDesc,
        Priority = SelectedPriority
    };
    
    private bool IsFormCorrect()
    {
        this.ValidateAllProperties();
        return !HasErrors;
    }

    public async Task Initialize(int id)
    {
        var bug = await bugService.GetAsync(id);
        Priorities = await priorityService.GetAllAsync();
        this.Id = bug.Id;
        this.Title = bug.Title;
        this.ShortDesc = bug.ShortDescription;
        this.Description = bug.Description;
        this.SelectedPriority = bug.Priority;
    }

    [RelayCommand(CanExecute = nameof(IsFormCorrect))]
    private async Task Submit()
    {
        try
        {
            await bugService.UpdateAsync(dto, Id);
            await Shell.Current.GoToAsync("../");
        }
        catch (Exception ex)
        {
            WeakReferenceMessenger.Default.Send(ex.Message);
        }
    }
    
}