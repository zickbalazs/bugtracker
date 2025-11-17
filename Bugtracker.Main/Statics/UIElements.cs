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