using System.Net;

namespace Bugtracker.Main.Statics;

public static class AuthData
{
    public const string SecureStorageUserEmailKey = "userEmail";
    private const string DatabaseUrl = "ginal.duckdns.org";

    public static string GetEmail()
    {
        return Preferences.Get(SecureStorageUserEmailKey, string.Empty);
    }

    public static void SetEmail(string email)
    {
        Preferences.Set(SecureStorageUserEmailKey, email);
    }
}