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
                    "Server=ginal.duckdns.org;Port=5432;Database=bugtracker;User Id=bugtracker;Password=bugtracker;"))
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
            .AddTransient<EditBugViewModel>();
        
        // PAGES
        builder.Services
            .AddSingleton<LoginPage>()
            .AddSingleton<RegistrationPage>()
            .AddSingleton<AllBugsPage>()
            .AddSingleton<PrioritiesMainPage>()
            .AddSingleton<UserDataPage>();
        
        // SUBPAGES | BUGS
        builder.Services
            .AddSingleton<BugDetailsPage>()
            .AddSingleton<CreateBugPage>()
            .AddSingleton<EditBugPage>();
        // SUBPAGES | COMMENTS
        
        // SUBPAGES | PRIORITIES
        
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}