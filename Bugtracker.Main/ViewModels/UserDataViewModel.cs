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



    private async Task GetCurrentUser()
    {
        var usr = await _service.GetUserByEmail(await SecureStorage.GetAsync("userEmail"));
        this.User = usr;
        this.IsLoading = false;
    }
}