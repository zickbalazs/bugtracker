using System.ComponentModel.DataAnnotations;
using Bugtracker.Main.Statics;
using Bugtracker.Main.Views.Auth;
using Bugtracker.Models;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace Bugtracker.Main.ViewModels;

public partial class CreateBugViewModel : ObservableValidator
{
    private readonly IBugService _bugService;
    private readonly IPriorityService _priorityService;
    private readonly IUserService _userService;

    [ObservableProperty, 
     NotifyCanExecuteChangedFor(nameof(UploadEntryCommand))] 
    private bool internetIsAvailable = Connectivity.NetworkAccess == NetworkAccess.Internet;
    
    [Required]
    [MinLength(3)]
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(UploadEntryCommand))]
    private string title = string.Empty;
    
    [Required]
    [MinLength(10)]
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(UploadEntryCommand))]
    private string shortDesc = string.Empty;
    
    [Required]
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(UploadEntryCommand))]
    private string details = string.Empty;
    
    [Required]
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(UploadEntryCommand))]
    private Priority? assignedPriority = null;

    [ObservableProperty] 
    private List<Priority> priorities = [];

    [ObservableProperty, NotifyPropertyChangedFor(nameof(CanClearFile))] 
    private FileResult? uploadedFile = null;
    
    [ObservableProperty]
    private bool isLoading = true;

    public bool CanClearFile => UploadedFile != null;
    
    public CreateBugViewModel(IBugService bugService, IPriorityService priorityService, IUserService userService)
    {
        _bugService = bugService;
        _priorityService = priorityService;
        _userService = userService;
        Connectivity.ConnectivityChanged +=
            (_, args) => InternetIsAvailable = args.NetworkAccess == NetworkAccess.Internet;
    }

    public async Task InitViewModel()
    {
        IsLoading = true;

        Priorities = [..await _priorityService.GetAllAsync()];

        IsLoading = false;
    }
    
    public bool IsFormCorrect
    {
        get
        {
            this.ValidateAllProperties();
            return !HasErrors && InternetIsAvailable;
        }
    }

    public BugDto BugDto =>
        new BugDto
        {
            Title = this.Title,
            Description = this.Details,
            ShortDescription = this.ShortDesc,
            Priority = this.AssignedPriority,
            FileName = UploadedFile?.FileName
        };

    [RelayCommand]
    private async Task UploadFile()
    {
        var filePicked = await FilePicker.Default.PickAsync();

        if (filePicked != null)
        {
            UploadedFile = filePicked;
        }
    }

    [RelayCommand]
    private void EmptyFile()
    {
        this.UploadedFile = null;
    }

    private void EmptyForm()
    {
        this.Title = string.Empty;
        this.ShortDesc = string.Empty;
        this.Details = string.Empty;
        this.AssignedPriority = null;
    }
    
    [RelayCommand(CanExecute = nameof(IsFormCorrect))]
    private async Task UploadEntry()
    {
        try
        {
            var userKey = AuthData.GetEmail();

            if (userKey == string.Empty)
            {
                Application.Current!.Windows[0].Page = new LoginShell();
            }
            else
            {
                if (UploadedFile != null)
                {
                    var result = await WebHelper.UploadImage(UploadedFile);
                    this.UploadedFile.FileName = result;
                }
                var user = await _userService.GetUserByEmail(userKey);
                await _bugService.CreateAsync(BugDto, user.Id);
                EmptyForm();
                await Shell.Current.GoToAsync("..//");
            }
        }
        catch (Exception ex)
        {
            WeakReferenceMessenger.Default.Send(ex.Message);
        }
    }
}