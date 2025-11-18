using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bugtracker.Main.ViewModels;

namespace Bugtracker.Main.Views.Priorities;

public partial class AddPriorityPage : ContentPage
{
    private PriorityAddViewModel vm;
    
    
    public AddPriorityPage(PriorityAddViewModel vm)
    {
        this.vm = vm;
        InitializeComponent();
        BindingContext = vm;
    }
}