using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bugtracker.Main.ViewModels;

namespace Bugtracker.Main.Views.Bugs;

public partial class BugDetailsPage : ContentPage, IQueryAttributable
{
    public BugDetailsPage(BugDetailsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var vm = (BugDetailsViewModel)BindingContext;
        var id = (string)query["id"];

        vm.InitDetails(Convert.ToInt32(id));
        
    }
}