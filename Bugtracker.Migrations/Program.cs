using Bugtracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bugtracker.Migrations;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Configuration.AddUserSecrets<Program>();
        builder.Services.AddDbContext<BugtrackerContext>(opt =>
        {
            opt.UseNpgsql(builder.Configuration["ConnectionStrings:BugtrackerConnection"]);
        });
        
        var app = builder.Build();
        
        app.Run();
    }
}