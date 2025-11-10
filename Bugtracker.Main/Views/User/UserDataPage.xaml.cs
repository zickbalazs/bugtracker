using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bugtracker.Main.ViewModels;

namespace Bugtracker.Main.Views.User;

public partial class UserDataPage : ContentPage
{
    public UserDataPage(UserDataViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}