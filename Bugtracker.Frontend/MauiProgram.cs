using Bugtracker.Data;
using Bugtracker.Frontend.Services;
using Bugtracker.Frontend.ViewModels;
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
        builder.Services.AddDbContext<BugtrackerContext>(opt=>
            opt.UseNpgsql(builder.Configuration["ConnectionStrings:BugtrackerConnection"]));
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<LoginPageViewModel>();
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}