using Bugtracker.Main.Statics;
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

        userAuthSuccess.Wait();
        
        return new Window(userAuthSuccess.Result ? new AppShell() : new LoginShell());
    }

    private async Task<bool> CheckIfLoggedIn()
    {
        return await SecureStorage.Default.GetAsync(AuthData.SecureStorageUserEmailKey) != null;
    }
    
}