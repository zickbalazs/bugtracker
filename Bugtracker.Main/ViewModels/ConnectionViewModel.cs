using Bugtracker.Main.Statics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bugtracker.Main.ViewModels;

public partial class ConnectionViewModel : ObservableObject
{

    public ConnectionViewModel()
    {
        Connectivity.ConnectivityChanged += CheckInternet;
    }

    private void CheckInternet(object? o, ConnectivityChangedEventArgs args)
    {
        Snackbar.Make(
            message: InternetPossible(args.NetworkAccess) ? 
                ConnectionSnackbar.EstablishedText : ConnectionSnackbar.LostText,
            visualOptions: InternetPossible(args.NetworkAccess) ? 
                ConnectionSnackbar.EstablishedOptions : ConnectionSnackbar.LostOptions,
            duration: ConnectionSnackbar.Duration
        ).Show();
    }

    private bool InternetPossible(NetworkAccess access) => access == NetworkAccess.Internet;

}