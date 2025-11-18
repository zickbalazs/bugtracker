using System.Net;
using Bugtracker.Main.Views.Auth;

namespace Bugtracker.Main.Statics;

public static class AuthData
{
    public const string BackendUrl = "https://server.zick.hu";
    
    private const string SecureStorageUserEmailKey = "userEmail";

    public static string GetEmail()
    {
        return Preferences.Get(SecureStorageUserEmailKey, string.Empty);
    }

    public static void SetEmail(string email)
    {
        Preferences.Set(SecureStorageUserEmailKey, email);
    }

    public static void GoToLogin()
    {
        ClearEmail();
        Application.Current!.Windows[0].Page = new LoginShell();
    }
    
    public static void ClearEmail()
    {
        Preferences.Clear();
    }
}