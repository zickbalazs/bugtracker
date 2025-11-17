using Bugtracker.Main.Statics;
using Bugtracker.Main.ViewModels;
using Bugtracker.Main.Views.Auth;

namespace Bugtracker.Main;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var userAuthSuccess = this.CheckIfLoggedIn();

        return new Window(userAuthSuccess ? new AppShell() : new LoginShell());
    }

    private bool CheckIfLoggedIn()
    {
        return AuthData.GetEmail() != string.Empty;
    }
    
}