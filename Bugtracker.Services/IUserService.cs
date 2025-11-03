using Bugtracker.Models;
using Bugtracker.Models.DTOs;

namespace Bugtracker.Services;

public interface IUserService
{
    public Task DeleteUserAsync(int id);
    
    public Task<bool> LoginAsync(LoginForm loginData);
    
    public Task RegisterAsync(RegistrationForm registrationData);
    
    public Task<bool> AuthorizeAsync(User user);
}