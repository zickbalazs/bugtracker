using Bugtracker.Data;
using Bugtracker.Main.Services;
using Bugtracker.Main.ViewModels;
using Bugtracker.Main.Views.Auth;
using Bugtracker.Main.Views.Bugs;
using Bugtracker.Main.Views.Priorities;
using Bugtracker.Main.Views.User;
using Bugtracker.Services;
using CommunityToolkit.Maui;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;

namespace Bugtracker.Main;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.ConfigureSyncfusionToolkit();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // SERVICES | DATABASE CONNECTION
        builder.Services
            .AddDbContext<BugtrackerContext>(opt =>
                opt.UseNpgsql(
                    "Server=zick.hu;Port=5432;Database=bugtracker;User Id=bugtracker;Password=bugtracker;"))
            .AddScoped<IUserService, DbUserService>()
            .AddScoped<IBugService, DbBugsService>()
            .AddScoped<IBugCommentService, DbCommentService>()
            .AddScoped<IPriorityService, DbPriorityService>();
        
        // VIEWMODELS
        builder.Services
            .AddTransient<LoginViewModel>()
            .AddTransient<RegistrationViewModel>()
            .AddTransient<BugsViewModel>()
            .AddTransient<BugDetailsViewModel>()
            .AddTransient<UserDataViewModel>()
            .AddTransient<CreateBugViewModel>()
            .AddTransient<EditBugViewModel>()
            .AddTransient<PrioritiesViewModel>()
            .AddTransient<PriorityAddViewModel>()
            .AddTransient<EditPriorityViewModel>();
        
        // PAGES
        builder.Services
            .AddSingleton<LoginPage>()
            .AddSingleton<RegistrationPage>()
            .AddTransient<AllBugsPage>()
            .AddTransient<PrioritiesMainPage>()
            .AddTransient<UserDataPage>();
        
        // SUBPAGES | BUGS
        builder.Services
            .AddTransient<BugDetailsPage>()
            .AddSingleton<CreateBugPage>()
            .AddTransient<EditBugPage>();
        // SUBPAGES | COMMENTS
        
        // SUBPAGES | PRIORITIES
        builder.Services
            .AddTransient<AddPriorityPage>()
            .AddTransient<EditPriorityPage>();
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}