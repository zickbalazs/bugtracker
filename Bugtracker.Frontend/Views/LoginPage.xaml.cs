using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bugtracker.Frontend.Viewmodels;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Frontend.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel vm)
    {
        BindingContext = vm;
        WeakReferenceMessenger.Default.Register<string>(this, (recipient, message) => 
            DisplayAlert("An error has occured", message, "OK"));
        InitializeComponent();
    }
}