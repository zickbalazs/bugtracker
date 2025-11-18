using Bugtracker.Main.Statics;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Main.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private bool internetIsAvailable = Connectivity.NetworkAccess == NetworkAccess.Internet;
    
    
    [ObservableProperty] private string emailAddress = string.Empty;
    [ObservableProperty] private string password = string.Empty;
    [ObservableProperty] private bool isLoading = false;
    private readonly IUserService _service;

    public LoginViewModel(IUserService service)
    {
        _service = service;
        Connectivity.ConnectivityChanged +=
            (_, args) => InternetIsAvailable = args.NetworkAccess == NetworkAccess.Internet;
    }

    private LoginForm Form => new()
    {
        Email = EmailAddress,
        Password = Password
    };

    [RelayCommand(CanExecute = nameof(InternetIsAvailable))]
    private async Task Login()
    {
        IsLoading = true;
        try
        {
            var successfulAttempt = await _service.LoginAsync(Form);
            
            if (successfulAttempt)
            {
                AuthData.SetEmail(Form.Email);
                Application.Current!.Windows[0].Page = new AppShell();
            }
            else
                WeakReferenceMessenger.Default.Send("Credentials are invalid");
        }
        catch (Exception e)
        {
            WeakReferenceMessenger.Default.Send(e.Message);
        }
        IsLoading = false;
    }
}