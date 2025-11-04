using Bugtracker.Frontend.ViewModels;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Frontend;

public partial class MainPage : ContentPage
{
    public MainPage(LoginPageViewModel viewmodel)
    {
        InitializeComponent();
        BindingContext = viewmodel;
        WeakReferenceMessenger.Default.Register<string>(this, (r, m) =>
        {
            DisplayAlert("An error has occured", m, "Ok");
        });
    }
}