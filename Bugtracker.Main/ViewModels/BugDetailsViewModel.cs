using Bugtracker.Main.Models;
using Bugtracker.Main.Statics;
using Bugtracker.Main.Views.Auth;
using Bugtracker.Models;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Main.ViewModels;

public partial class BugDetailsViewModel : ObservableObject
{
    private readonly IBugService _bugService;
    private readonly IBugCommentService _commentService;
    private readonly IUserService _userService;

    
    [ObservableProperty, 
    NotifyCanExecuteChangedFor(nameof(OpenFileInBrowserCommand)),
    NotifyCanExecuteChangedFor(nameof(CommentCommand)),
    NotifyCanExecuteChangedFor(nameof(MarkAsDoneCommand))]
    private bool internetIsAvailable = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;

    [ObservableProperty,
     NotifyCanExecuteChangedFor(nameof(MarkAsDoneCommand)),
     NotifyCanExecuteChangedFor(nameof(CommentCommand))]
    private User currentUser = new();

    [ObservableProperty,
     NotifyCanExecuteChangedFor(nameof(MarkAsDoneCommand)),
     NotifyCanExecuteChangedFor(nameof(CommentCommand)),
     NotifyPropertyChangedFor(nameof(FileExists))]
    private ObservableBug bug = new()
    {
        Title = string.Empty,
        Description = string.Empty,
        ShortDescription = string.Empty,
        AssociatedFileName = "",
        Author = new User() { },
        Comments = [],
        Created = DateTime.Now,
        Id = -1,
        Priority = new Priority() { Id = -1, ColorCode = "#fff", Title = string.Empty },
        Solved = false,
        SolvedOn = null,
    };

    [ObservableProperty,
     NotifyCanExecuteChangedFor(nameof(MarkAsDoneCommand)),
     NotifyCanExecuteChangedFor(nameof(CommentCommand))]
    private bool isLoading;
    [ObservableProperty, NotifyPropertyChangedFor(nameof(HasComments))]
    private List<BugComment> comments = [];
    [ObservableProperty] private string pageTitle = "Loading Bug Details...";


    private Bug dtoBug => new()
    {
        Title = Bug.Title,
        Description = Bug.Description,
        ShortDescription = Bug.ShortDescription,
        AssociatedFileName = Bug.AssociatedFileName,
        Author = Bug.Author,
        Created = Bug.Created,
        Id = Bug.Id,
        Priority = Bug.Priority,
        Solved = Bug.Solved,
        SolvedOn = Bug.SolvedOn
    };
    
    public bool HasComments => Comments.Count > 0;

    private bool AllowedToComment()
    {
        return !IsLoading && !Bug.Solved && InternetIsAvailable;
    }

    public bool FileExists => Bug.AssociatedFileName != null;
    
    private bool AllowedToClose()
    {
        return !IsLoading && 
               !Bug.Solved && 
               (Bug.Author.Id == CurrentUser.Id || CurrentUser.IsAdmin) && 
               InternetIsAvailable;
    }

    public BugDetailsViewModel(IBugService bugService, IBugCommentService commentService, IUserService userService)
    {
        _bugService = bugService;
        _commentService = commentService;
        _userService = userService;
        Connectivity.ConnectivityChanged +=
            (_, args) => InternetIsAvailable = args.NetworkAccess == NetworkAccess.Internet;
    }

    public async Task InitDetails(int id)
    {
        IsLoading = true;
        await GetUser();
        Bug = ObservableBug.Parse(await _bugService.GetAsync(id));
        Comments = [ ..await _commentService.GetByBugAsync(dtoBug) ];
        PageTitle = $"#{Bug.Id} {Bug.Title}";
        IsLoading = false;
    }

    private async Task GetUser()
    {
        var email = AuthData.GetEmail();

        if (email == string.Empty)
            Application.Current!.Windows[0].Page = new LoginShell();

        else
        {
            CurrentUser = await _userService.GetUserByEmail(email);
        }
    }

    [RelayCommand(CanExecute = nameof(AllowedToClose))]
    private async Task MarkAsDone()
    {
        try
        {
            await _bugService.MarkBugAsSolved(Bug.Id);
            await this.InitDetails(Bug.Id);
        }
        catch (Exception ex)
        {
            WeakReferenceMessenger.Default.Send(ex.Message);
        }
        
    }

    [RelayCommand(CanExecute = nameof(AllowedToComment))]
    private async Task Comment()
    {
        var result = await Application.Current!.MainPage.DisplayPromptAsync(
            title: UIElements.CommentPrompt.Title,
            message: UIElements.CommentPrompt.Message,
            accept: UIElements.CommentPrompt.ConfirmText,
            cancel: UIElements.CommentPrompt.CancelText,
            placeholder: UIElements.CommentPrompt.PlaceholderText,
            maxLength: UIElements.CommentPrompt.MaxLength,
            keyboard: Keyboard.Chat,
            initialValue: UIElements.CommentPrompt.InitialValue
        );
        if (result == null) WeakReferenceMessenger.Default.Send("Reply must have contents");
        else
        {
            try
            {
                await _commentService.CreateAsync(result, CurrentUser, await _bugService.GetAsync(Bug.Id));
                await this.InitDetails(Bug.Id);
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(ex.Message);
            }
        }
    }
    [RelayCommand(CanExecute = nameof(InternetIsAvailable))]
    private async Task OpenFileInBrowser()
    {
        await Browser.Default.OpenAsync($"{AuthData.BackendUrl}/storage/{Bug.AssociatedFileName}");
    }
}