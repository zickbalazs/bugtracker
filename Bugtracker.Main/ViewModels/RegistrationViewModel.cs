using System.ComponentModel.DataAnnotations;
using Bugtracker.Models;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Main.ViewModels;

public partial class RegistrationViewModel(IUserService service) : ObservableValidator
{
    [ObservableProperty] 
    private bool isLoading = false;
    
    [ObservableProperty]
    [Required]
    [MinLength(8)]
    [NotifyPropertyChangedFor(nameof(FormValid))]
    private string password = string.Empty;
    
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

    public bool FormValid
    {
        get
        {
            this.ValidateAllProperties();
            return !this.HasErrors;
        }
    }

    private RegistrationForm Form => new RegistrationForm
    {
        Email = this.Email,
        Username = this.Username,
        Password = this.Password
    };

    [RelayCommand]
    private async Task RegisterUser()
    {
        IsLoading = true;
        try
        {
            this.ValidateAllProperties();
            if (HasErrors)
            {
                WeakReferenceMessenger.Default.Send(this.GetErrors());
            }
            else
            {
                await service.RegisterAsync(this.Form);
                await SecureStorage.Default.SetAsync("userEmail", this.Email);
                Application.Current!.Windows[0].Page = new AppShell();
            }
        }
        catch (Exception e)
        {
            WeakReferenceMessenger.Default.Send(e.Message);
        }
        IsLoading = false;
    }
}