using Bugtracker.Main.Statics;
using Bugtracker.Main.Views.Auth;
using Bugtracker.Models;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Main.ViewModels;

public partial class UserDataViewModel : ObservableObject
{
    private readonly IUserService _service;
    
    [ObservableProperty] private User user;
    [ObservableProperty] private bool isLoading = true;
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    private bool internetAvailable = Connectivity.NetworkAccess == NetworkAccess.Internet;
    
    
    public UserDataViewModel(IUserService service)
    {
        _service = service;
        Connectivity.ConnectivityChanged +=
            (_, args) => InternetAvailable = args.NetworkAccess == NetworkAccess.Internet; 
        GetCurrentUser();
    }

    [RelayCommand]
    private void Logout()
    {
        Application.Current!.Windows[0].Page = new LoginShell();
        Preferences.Clear();
    }

    [RelayCommand(CanExecute = nameof(InternetAvailable))]
    private async Task Delete()
    {
        var result = await Application.Current.MainPage.DisplayAlert(
            title: ConfirmationDialog.Title,
            message: ConfirmationDialog.Message,
            accept: ConfirmationDialog.Confirm,
            cancel: ConfirmationDialog.Cancel);

        if (result)
        {
            try
            {
                await _service.DeleteUserAsync(User.Id);
                Logout();
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(ex.Message);
            }           
        }
    }

    public async Task GetCurrentUser()
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