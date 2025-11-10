using Bugtracker.Data;
using Bugtracker.Main.Services;
using Bugtracker.Main.ViewModels;
using Bugtracker.Main.Views.Auth;
using Bugtracker.Main.Views.Bugs;
using Bugtracker.Main.Views.Priorities;
using Bugtracker.Main.Views.User;
using Bugtracker.Services;
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
            .AddScoped<IUserService, DbUserService>();
        
        // VIEWMODELS
        builder.Services
            .AddTransient<LoginViewModel>()
            .AddTransient<RegistrationViewModel>()
            .AddTransient<BugsViewModel>()
            .AddTransient<BugDetailsViewModel>()
            .AddTransient<UserDataViewModel>();
        
        // PAGES
        builder.Services
            .AddSingleton<LoginPage>()
            .AddSingleton<RegistrationPage>()
            .AddSingleton<AllBugsPage>()
            .AddSingleton<UnsolvedBugsPage>()
            .AddSingleton<PrioritiesMainPage>()
            .AddSingleton<UserDataPage>();
        
        // SUBPAGES | BUGS
        builder.Services.AddSingleton<BugDetailsPage>();
        // SUBPAGES | COMMENTS
        
        // SUBPAGES | PRIORITIES
        
#if DEBUG
        SecureStorage.Default.RemoveAll();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}