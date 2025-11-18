using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bugtracker.Main.ViewModels;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Main.Views.Auth;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        WeakReferenceMessenger.Default.Register<string>(this, (_, message) => DisplayAlert("An error has occured", message, "OK"));
    }
}