using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bugtracker.Frontend.Viewmodels;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Frontend.Views;

public partial class RegistrationPage : ContentPage
{
    public RegistrationPage(RegistrationViewModel vm)
    {
        BindingContext = vm;
        WeakReferenceMessenger.Default.Register<string>(this, (obj, message) => 
            DisplayAlert("An error has occured", message, "OK"));
        InitializeComponent();
    }
}