using System.ComponentModel.DataAnnotations;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Main.ViewModels;

public partial class PriorityAddViewModel : ObservableValidator
{
    private readonly IPriorityService _service;
    
    [ObservableProperty,
     NotifyCanExecuteChangedFor(nameof(CreateCommand)),
     Required]
    private string title = string.Empty;

    [ObservableProperty,
     NotifyCanExecuteChangedFor(nameof(CreateCommand)),
     Required,
     RegularExpression(@"^#?([0-9a-f]{6}|[0-9a-f]{3})$")]
    private string colorCode = string.Empty;

    private PriorityDto dto => new PriorityDto
    {
        Title = this.Title,
        ColorCode = this.ColorCode
    };
    
    private bool CanCreate()
    {
        ValidateAllProperties();
        return !HasErrors;
    }
    
    public PriorityAddViewModel(IPriorityService service)
    {
        _service = service;
    }

    [RelayCommand(CanExecute = nameof(CanCreate))]
    private async Task Create()
    {
        try
        {
            await _service.CreateAsync(dto);
            EmptyForm();
            await Shell.Current.GoToAsync("../");
        }
        catch (Exception ex)
        {
            WeakReferenceMessenger.Default.Send(ex.Message);
        }
    }

    private void EmptyForm()
    {
        this.ColorCode = string.Empty;
        this.Title = string.Empty;
    }
    
}