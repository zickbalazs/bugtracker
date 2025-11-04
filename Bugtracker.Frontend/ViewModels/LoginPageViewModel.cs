using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Frontend.ViewModels;

public partial class LoginPageViewModel : ObservableObject
{
    private readonly IUserService userService;
    
    public LoginPageViewModel(IUserService userService)
    {
        // DI
        this.userService = userService;

        // Setting properties
        InternetConnectionEstablished = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        LoginForm = new LoginForm
        {
            Email = "",
            Password = "",
        };
        
        // Event subscription for network access check.
        Connectivity.Current.ConnectivityChanged += (obj, args) => SetConnectivityStatus(args);
    }

    private void SetConnectivityStatus(ConnectivityChangedEventArgs args)
    {
        InternetConnectionEstablished = args.NetworkAccess == NetworkAccess.Internet;
    }


    [ObservableProperty] 
    private LoginForm loginForm;

    [ObservableProperty] 
    private bool internetConnectionEstablished;

    [RelayCommand]
    public async Task TryLoginAsync()
    {
        try
        {
            var loginValid = await userService.LoginAsync(LoginForm);
            if (loginValid)
            {
                Preferences.Default.Set("authorizedUserEmail", LoginForm.Email);
            }
            else
            {
                WeakReferenceMessenger.Default.Send($"Login attempt failed!");
            }
        }
        catch (Exception e)
        {
            WeakReferenceMessenger.Default.Send(e.Message);
        }
    }
}