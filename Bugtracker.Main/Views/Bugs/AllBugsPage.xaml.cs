using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bugtracker.Main.ViewModels;
using Bugtracker.Models;

namespace Bugtracker.Main.Views.Bugs;

public partial class AllBugsPage : ContentPage
{
    public AllBugsPage(BugsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ((BugsViewModel)BindingContext).GetBugs();
    }
}