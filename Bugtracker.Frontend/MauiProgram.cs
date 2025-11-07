using Bugtracker.Data;
using Bugtracker.Frontend.Services;
using Bugtracker.Frontend.Viewmodels;
using Bugtracker.Frontend.Views;
using Bugtracker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bugtracker.Frontend;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.Configuration.AddUserSecrets<App>();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // SERVICES
        builder.Services.AddDbContext<BugtrackerContext>(opts =>
            opts.UseNpgsql(builder.Configuration["Connections:BugtrackerDb"]));
        builder.Services.AddScoped<IUserService, DbUserService>();
        
        // VIEWMODELS
        builder.Services.AddSingleton<ConnectionViewModel>();
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<RegistrationViewModel>();
        // PAGES
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<RegistrationPage>();
        builder.Services.AddSingleton<MainMenuPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}