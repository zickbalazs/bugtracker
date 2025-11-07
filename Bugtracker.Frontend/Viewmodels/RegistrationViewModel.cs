using System.ComponentModel.DataAnnotations;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Frontend.Viewmodels;

public partial class RegistrationViewModel(IUserService userService) : ObservableValidator
{
    [ObservableProperty]
    [Required]
    [EmailAddress]
    [NotifyPropertyChangedFor(nameof(FormValid))]
    private string email = string.Empty;

    [ObservableProperty]
    [Required]
    [MinLength(3)]
    [NotifyPropertyChangedFor(nameof(FormValid))]
    private string username = string.Empty;

    [ObservableProperty]
    [Required]
    [MinLength(8)]
    [NotifyPropertyChangedFor(nameof(FormValid))]
    private string password = string.Empty;

    public RegistrationForm Form => new RegistrationForm
    {
        Email = this.Email,
        Password = this.Password,
        Username = this.Username
    };

    public bool FormValid
    {
        get
        {
            ValidateAllProperties();
            return !this.HasErrors;
        }
    }


    [RelayCommand]
    public async Task RegisterUser()
    {
        try
        {
            await userService.RegisterAsync(Form);
        }
        catch (Exception e)
        {
            WeakReferenceMessenger.Default.Send(e.Message);
        }
    } 
}