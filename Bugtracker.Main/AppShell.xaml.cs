using Bugtracker.Main.Views.Bugs;
using Bugtracker.Main.Views.Priorities;

namespace Bugtracker.Main;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("createPriority", typeof(AddPriorityPage));
        Routing.RegisterRoute("bugdetails", typeof(BugDetailsPage));
        Routing.RegisterRoute("createBug", typeof(CreateBugPage));
        Routing.RegisterRoute("editBug", typeof(EditBugPage));
    }
}