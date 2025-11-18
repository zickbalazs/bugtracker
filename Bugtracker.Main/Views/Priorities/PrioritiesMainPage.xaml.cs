using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bugtracker.Main.ViewModels;

namespace Bugtracker.Main.Views.Priorities;

public partial class PrioritiesMainPage : ContentPage
{
    public PrioritiesMainPage(PrioritiesViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        ((PrioritiesViewModel)BindingContext).GetPriorities();
    }
}