using CommunityToolkit.Mvvm.ComponentModel;

namespace Bugtracker.Frontend.Viewmodels;

public partial class ConnectionViewModel : ObservableObject
{
    [ObservableProperty] 
    private bool isInternetAvailable = Connectivity.NetworkAccess == NetworkAccess.Internet;

    public ConnectionViewModel()
    {
        SubscribeToConnectivityChange();
    }

    private void SubscribeToConnectivityChange()
    {
        Connectivity.ConnectivityChanged += (obj, newConnection) =>
            IsInternetAvailable = newConnection.NetworkAccess == NetworkAccess.Internet;
    }
}