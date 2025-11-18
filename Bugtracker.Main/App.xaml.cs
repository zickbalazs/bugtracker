using Bugtracker.Main.Statics;
using Bugtracker.Main.ViewModels;
using Bugtracker.Main.Views.Auth;
using Bugtracker.Services;

namespace Bugtracker.Main;

public partial class App : Application
{
    private readonly IUserService _service;
    
    public App(IUserService service)
    {
        _service = service;
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