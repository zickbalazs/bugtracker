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
     NotifyCanExecuteChangedFor(nameof(MarkAsDoneCommand)),
     NotifyCanExecuteChangedFor(nameof(CommentCommand)),
     NotifyCanExecuteChangedFor(nameof(EditCommentCommand)),
     NotifyCanExecuteChangedFor(nameof(DeleteCommentCommand))]
    private User currentUser= new User();
    
    [ObservableProperty, 
     NotifyCanExecuteChangedFor(nameof(MarkAsDoneCommand)),
     NotifyCanExecuteChangedFor(nameof(CommentCommand))]
    private Bug bug = new Bug()
    {
        Title = "Loading",
        Description = "Loading",
        ShortDescription = "Loading",
        Solved = false,
        SolvedOn = DateTime.Now,
        Author = new User()
    };
    
    [ObservableProperty, 
     NotifyCanExecuteChangedFor(nameof(MarkAsDoneCommand)), 
     NotifyCanExecuteChangedFor(nameof(CommentCommand)),
     NotifyCanExecuteChangedFor(nameof(EditCommentCommand))]
    private bool isLoading;
    [ObservableProperty, NotifyPropertyChangedFor(nameof(HasComments))]
    private List<BugComment> comments = [];
    [ObservableProperty] private string pageTitle = "Loading Bug Details...";
    
    
    public bool HasComments => Comments.Count > 0;

    private bool AllowedToComment()
    {
        return !IsLoading && !Bug.Solved;
    }

    private bool AllowedToClose()
    {
        return !IsLoading && !Bug.Solved && (Bug.Author.Id == CurrentUser.Id || CurrentUser.IsAdmin);
    }

    private bool AllowedToEditComment(BugComment comment)
    {
        return !IsLoading && comment.Author.Id == CurrentUser.Id;
    }

    private bool AllowedToDeleteComment(BugComment comment)
    {
        return !IsLoading && (comment.Author.Id == CurrentUser.Id || CurrentUser.IsAdmin);
    }
    
    public BugDetailsViewModel(IBugService bugService, IBugCommentService commentService, IUserService userService)
    {
        _bugService = bugService;
        _commentService = commentService;
        _userService = userService;
    }

    public async Task InitDetails(int id)
    {
        IsLoading = true;
        await GetUser();
        Bug = await _bugService.GetAsync(id);
        Comments = [ ..await _commentService.GetByBugAsync(Bug) ];
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
                await _commentService.CreateAsync(result, CurrentUser, Bug);
                await this.InitDetails(Bug.Id);
            }
            catch (Exception ex)
            {
                WeakReferenceMessenger.Default.Send(ex.Message);
            }
        }
    }

    [RelayCommand(CanExecute = nameof(AllowedToEditComment))]
    private async Task EditComment(BugComment comment)
    {
        
    }

    [RelayCommand(CanExecute = nameof(AllowedToDeleteComment))]
    private async Task DeleteComment(BugComment comment)
    {
        
    }
}