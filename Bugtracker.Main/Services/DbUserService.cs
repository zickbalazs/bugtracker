using System.Data;
using System.Security.Cryptography;
using System.Text;
using Bugtracker.Data;
using Bugtracker.Models;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using Microsoft.EntityFrameworkCore;

namespace Bugtracker.Main.Services;

public class DbUserService(BugtrackerContext ctx) : IUserService
{
    public Task DeleteUserAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> LoginAsync(LoginForm loginData)
    {
        return await ctx.Users.FirstOrDefaultAsync(x => 
            x.Email == loginData.Email && 
            x.Password == HashString(loginData.Password)) != null;
    }

    public async Task RegisterAsync(RegistrationForm registrationData)
    {
        var emailDuplicateDetected = 
            await ctx.Users.FirstOrDefaultAsync(x => x.Email == registrationData.Email) != null;

        if (emailDuplicateDetected)
            throw new DuplicateNameException("This email address is already in use.");

        var usernameDuplicateDetected =
            await ctx.Users.FirstOrDefaultAsync(x => x.Name == registrationData.Username) != null;

        if (usernameDuplicateDetected)
            throw new DuplicateNameException("This username is already in use.");

        await ctx.Users.AddAsync(new User
        {
            Email = registrationData.Email,
            Name = registrationData.Username,
            Password = HashString(registrationData.Password),
            IsAdmin = !ctx.Users.Any(x=>x.IsAdmin)
        });
        await ctx.SaveChangesAsync();
    }

    public Task<bool> AuthorizeAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetUserByEmail(string emailAddress)
    {
        return await ctx.Users.FirstAsync(x => x.Email == emailAddress);
    }


    private string HashString(string text)
    {
        return BitConverter.ToString(SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(text)));
    }
}