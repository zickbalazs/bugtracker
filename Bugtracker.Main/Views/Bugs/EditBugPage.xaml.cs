using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bugtracker.Main.ViewModels;

namespace Bugtracker.Main.Views.Bugs;

public partial class EditBugPage : ContentPage, IQueryAttributable
{
    public EditBugPage(EditBugViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var id = Convert.ToInt32(query["id"]);
        var vm = (EditBugViewModel)BindingContext;
        vm.Initialize(id);
    }
}