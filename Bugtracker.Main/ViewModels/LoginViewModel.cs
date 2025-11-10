using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Main.ViewModels;

public partial class LoginViewModel(IUserService service) : ObservableObject
{
    [ObservableProperty] private string emailAddress = string.Empty;
    [ObservableProperty] private string password = string.Empty;
    [ObservableProperty] private bool isLoading = false;
    
    private LoginForm Form => new()
    {
        Email = EmailAddress,
        Password = Password
    };

    [RelayCommand]
    private async Task Login()
    {
        IsLoading = true;
        try
        {
            var successfulAttempt = await service.LoginAsync(Form);
            
            if (successfulAttempt)
            {
                await SecureStorage.Default.SetAsync("userEmail", Form.Email);
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