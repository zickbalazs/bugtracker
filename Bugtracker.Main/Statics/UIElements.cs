using CommunityToolkit.Maui.Core;
using Markdig.Helpers;

namespace Bugtracker.Main.Statics;

public static class UIElements
{
    //ALERT DIALOG
    public const string ConfirmDialogText = "Ok";
    public const string ErrorDialogTitle = "An error has occured.";
    
    //TEXT SIZES
    public const double H1TextSize = 32;
    public const double H2TextSize = 24;
    public const double H3TextSize = 18;
    public const double TextSize = 14;
    
    //FONT
    public const string TextFont = "OpenSansRegular";

    public struct CommentPrompt
    {
        public const string Title = "Write a reply";
        public const string ConfirmText = "Send";
        public const string CancelText = "Cancel";
        public const string PlaceholderText = "Write your thoughts here.";
        public const string Message = "";
        public const string InitialValue = "";
        public const int MaxLength = -1;
    }

    
}
public struct PriorityStyles
{
    public const double Size = 10;
}

public struct ConfirmationDialog
{
    public const string Title = "Confirm Account Deletion";
    public const string Message =
        "Are you sure you want to delete your account?\nEvery data associated to this account will be deleted!";

    public const string Confirm = "Yes";
    public const string Cancel = "No";
}

public static class ConnectionSnackbar
{
    public const string EstablishedText = "Internet connection established!";
    public const string LostText = "Internet connection unsuccessful!";
    public static readonly TimeSpan Duration = new(0,0,2);
    public static readonly SnackbarOptions EstablishedOptions = new()
    {
        BackgroundColor = Color.FromArgb("#28a745"),
        TextColor = Colors.White,
        ActionButtonTextColor = Colors.White
    };

    public static readonly SnackbarOptions LostOptions = new()
    {
        BackgroundColor = Color.FromArgb("#dc3545"),
        TextColor = Colors.White,
        ActionButtonTextColor = Colors.White
    };
}