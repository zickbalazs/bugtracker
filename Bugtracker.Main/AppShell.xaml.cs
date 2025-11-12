using Bugtracker.Main.Views.Bugs;

namespace Bugtracker.Main;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("bugdetails", typeof(BugDetailsPage));
        Routing.RegisterRoute("createBug", typeof(CreateBugPage));
    }
}