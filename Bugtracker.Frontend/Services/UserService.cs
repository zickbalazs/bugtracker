using System.Data;
using Bugtracker.Data;
using Bugtracker.Models;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using Microsoft.EntityFrameworkCore;

namespace Bugtracker.Frontend.Services;

public class UserService(BugtrackerContext context) : IUserService
{
    public async Task DeleteUserAsync(int id)
    {
        await context.Users.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task<bool> LoginAsync(LoginForm loginData)
    {
        var userToFind = await context.Users.FirstOrDefaultAsync(
            x => x.Email == loginData.Email && x.Password == loginData.Password);

        return userToFind == null;
    }

    public async Task RegisterAsync(RegistrationForm registrationData)
    {
        var userWithSameEmail = await context.Users.FirstOrDefaultAsync(x => x.Email == registrationData.Email);

        if (userWithSameEmail != null)
            throw new DuplicateNameException("user with this email already exists.");

        await context.Users.AddAsync(new User
        {
            Email = registrationData.Email,
            Password = registrationData.Password,
            Name = registrationData.Username,
            IsAdmin = await context.Users.CountAsync() <= 0,
        });

        await context.SaveChangesAsync();
    }

    public async Task<bool> AuthorizeAsync(User user)
    {
        return (await context.Users.FindAsync(user))!.IsAdmin;
    }
}