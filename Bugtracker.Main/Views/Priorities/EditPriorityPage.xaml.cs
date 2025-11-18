using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bugtracker.Main.ViewModels;

namespace Bugtracker.Main.Views.Priorities;

public partial class EditPriorityPage : ContentPage, IQueryAttributable
{
    public EditPriorityPage(EditPriorityViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var id = Convert.ToInt32(query["id"]);
        ((EditPriorityViewModel)BindingContext).GetPriority(id);
    }
}