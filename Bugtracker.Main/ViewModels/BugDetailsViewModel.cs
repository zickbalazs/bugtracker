using Bugtracker.Main.Statics;
using Bugtracker.Main.Views.Auth;
using Bugtracker.Models;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bugtracker.Main.ViewModels;

public partial class BugDetailsViewModel : ObservableObject
{
    private readonly IBugService _bugService;
    private readonly IBugCommentService _commentService;
    private readonly IUserService _userService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(AllowedToComment))]
    [NotifyPropertyChangedFor(nameof(AllowedToClose))]
    private User currentUser= new User();
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(AllowedToComment))]
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

    public bool AllowedToComment => !IsLoading && !Bug.Solved;
    public bool AllowedToClose => !IsLoading && 
                                  !Bug.Solved && 
                                  (Bug.Author.Id == CurrentUser.Id || CurrentUser.IsAdmin);
    
    
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
        var email = await SecureStorage.Default.GetAsync(AuthData.SecureStorageUserEmailKey);

        if (email == null)
            Application.Current!.Windows[0].Page = new LoginShell();

        else
        {
            CurrentUser = await _userService.GetUserByEmail(email);
        }
    }
    
}