using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bugtracker.Main.Statics;
using Bugtracker.Main.ViewModels;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Main.Views.Bugs;

public partial class CreateBugPage : ContentPage
{
    public CreateBugPage(CreateBugViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        WeakReferenceMessenger.Default.Register<string>(this, (recipient, message) => 
            DisplayAlert(title: UIElements.ErrorDialogTitle,
                         message: message,
                         cancel: UIElements.ConfirmDialogText));
    }
}