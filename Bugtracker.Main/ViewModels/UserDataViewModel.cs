using Bugtracker.Main.Statics;
using Bugtracker.Main.Views.Auth;
using Bugtracker.Models;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bugtracker.Main.ViewModels;

public partial class UserDataViewModel : ObservableObject
{
    private readonly IUserService _service;
    
    [ObservableProperty] private User user;
    [ObservableProperty] private bool isLoading = true;

    public UserDataViewModel(IUserService service)
    {
        _service = service;
        GetCurrentUser();
    }

    [RelayCommand]
    private void Logout()
    {
        Application.Current!.Windows[0].Page = new LoginShell();
        Preferences.Clear();
    }

    private async Task GetCurrentUser()
    {
        var email = AuthData.GetEmail();

        if (email == string.Empty)
        {
            Application.Current!.Windows[0].Page = new LoginShell();
        }
        else
        {
            var usr = await _service.GetUserByEmail(email);
            this.User = usr;
            this.IsLoading = false;    
        }
    }
}