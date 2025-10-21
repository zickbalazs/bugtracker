using Microsoft.Extensions.Hosting;

namespace Bugtracker.Migrations;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = new HostBuilder();
        
        var app = builder.Build();
        
        app.Run();
    }
}