using Bugtracker.Frontend.Viewmodels;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Frontend;
using Bugtracker.Frontend.Views;

public partial class App : Application
{
    private LoginViewModel loginVm;
    
    public App(LoginViewModel loginViewModel)
    {
        loginVm = loginViewModel;
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var authTask = SecureStorage.Default.GetAsync("userEmail");
        authTask.Wait();
        var authEmail = authTask.Result;

        return new Window(
            authEmail == null ? 
                new NavigationPage(new LoginPage(this.loginVm)) :
                new MainMenuPage()
        );



    }
}