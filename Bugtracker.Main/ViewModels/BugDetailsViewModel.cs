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

    [ObservableProperty]
    private User currentUser= new User();
    
    [ObservableProperty]
    private Bug bug = new Bug()
    {
        Title = "Loading",
        Description = "Loading",
        ShortDescription = "Loading",
        Solved = false,
        Author = new User()
    };
    
    [ObservableProperty]
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
        CommentCommand.NotifyCanExecuteChanged();
        MarkAsDoneCommand.NotifyCanExecuteChanged();
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
        
    }
}