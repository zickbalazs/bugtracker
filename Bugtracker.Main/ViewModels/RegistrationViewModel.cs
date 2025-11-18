using System.ComponentModel.DataAnnotations;
using Bugtracker.Main.Statics;
using Bugtracker.Models;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Main.ViewModels;

public partial class RegistrationViewModel : ObservableValidator
{
    [ObservableProperty] 
    private bool isLoading = false;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(RegisterUserCommand))]
    private bool internetIsAvailable = Connectivity.NetworkAccess == NetworkAccess.Internet;
    
    [ObservableProperty]
    [Required]
    [MinLength(8)]
    [NotifyCanExecuteChangedFor(nameof(RegisterUserCommand))]
    private string password = string.Empty;
    
    [ObservableProperty]
    [Required]
    [EmailAddress]
    [NotifyCanExecuteChangedFor(nameof(RegisterUserCommand))]
    private string email = string.Empty;
    
    [ObservableProperty]
    [Required]
    [MinLength(3)]
    [NotifyCanExecuteChangedFor(nameof(RegisterUserCommand))]
    private string username = string.Empty;

    private readonly IUserService _service;

    public RegistrationViewModel(IUserService service)
    {
        _service = service;
        Connectivity.ConnectivityChanged +=
            (_, args) => InternetIsAvailable = args.NetworkAccess == NetworkAccess.Internet;
    }

    public bool FormValid
    {
        get
        {
            this.ValidateAllProperties();
            return !this.HasErrors && InternetIsAvailable;
        }
    }

    private RegistrationForm Form => new RegistrationForm
    {
        Email = this.Email,
        Username = this.Username,
        Password = this.Password
    };

    [RelayCommand(CanExecute = nameof(FormValid))]
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
                await _service.RegisterAsync(this.Form);
                AuthData.SetEmail(Form.Email);
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