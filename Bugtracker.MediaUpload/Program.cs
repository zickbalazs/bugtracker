using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "storage")))
{
    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "storage"));
}

var app = builder.Build();

app.MapPost("/upload", (ctx) =>
{
    try
    {
        var file = ctx.Request.Form.Files.First();
        var filename = $"{DateTime.Now:yyyy-MM-dd-hhmmss}-{file.FileName}";
        using (var writer = file.OpenReadStream())
        {
            using (var copy = File.Open(
                       Path.Combine(Directory.GetCurrentDirectory(),
                           $"storage/{filename}"), FileMode.CreateNew))
            {
                writer.CopyTo(copy);
            }
        }

        return ctx.Response.WriteAsync(filename);
    }
    catch (Exception ex)
    {
        ctx.Response.StatusCode = 403;
        return ctx.Response.WriteAsync(ex.Message);
    }
});

app.UseFileServer(new FileServerOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "storage")),
    RequestPath = "/storage"
});

app.UseHttpsRedirection();
app.Run();