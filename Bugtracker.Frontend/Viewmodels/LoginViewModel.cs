using Bugtracker.Frontend.Views;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Frontend.Viewmodels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly RegistrationViewModel _registrationViewModel;
    
    [ObservableProperty]
    private LoginForm form = new LoginForm
    {
        Email = string.Empty,
        Password = string.Empty
    };

    public LoginViewModel(IUserService userService, RegistrationViewModel registrationViewModel)
    {
        _userService = userService;
        _registrationViewModel = registrationViewModel;
    }

    [RelayCommand]
    public async Task AttemptLogin()
    {
        try
        {
            var success = await _userService.LoginAsync(Form);

            if (success)
                Application.Current.MainPage = new MainMenuPage();

            else
                WeakReferenceMessenger.Default.Send("Bad login information!");
        }
        catch (Exception e)
        {
            WeakReferenceMessenger.Default.Send(e.Message);
        }
    }

    [RelayCommand]
    public async Task GoToRegistration()
    {
        await Application.Current.MainPage.Navigation.PushAsync(new RegistrationPage(_registrationViewModel));
    }
}