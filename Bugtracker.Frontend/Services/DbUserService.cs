using System.Data;
using System.Security.Cryptography;
using System.Text;
using Bugtracker.Data;
using Bugtracker.Models;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using Microsoft.EntityFrameworkCore;

namespace Bugtracker.Frontend.Services;

public class DbUserService(BugtrackerContext ctx) : IUserService
{
    public async Task DeleteUserAsync(int id)
    {
        await ctx.Users.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task<bool> LoginAsync(LoginForm loginData)
    {
        var hashedPass = SHA512StringHash(loginData.Password);

        return await ctx.Users.FirstOrDefaultAsync(x =>
            x.Email == loginData.Email &&
            x.Password == hashedPass) != null;
    }

    public async Task RegisterAsync(RegistrationForm registrationData)
    {
        bool emailConflict = ctx.Users.FirstOrDefault(x => x.Email == registrationData.Email) != null;
        
        if (emailConflict)
            throw new DuplicateNameException("The email address is already in use");
        
        bool userNameConflict = ctx.Users.FirstOrDefault(x => x.Name == registrationData.Username) != null;

        if (userNameConflict)
            throw new DuplicateNameException("The username must be unique!");
        
        await ctx.Users.AddAsync(new User
        {
            Email = registrationData.Email,
            IsAdmin = (await ctx.Users.CountAsync()) < 1,
            Name = registrationData.Username,
            Password = BitConverter.ToString(
                SHA512
                    .Create()
                    .ComputeHash(Encoding.UTF8.GetBytes(registrationData.Password)))
        });
        
        await ctx.SaveChangesAsync();
    }

    public async Task<bool> AuthorizeAsync(User user)
    {
        return (await ctx.Users.FirstAsync(x => x == user)).IsAdmin;
    }

    public async Task<User> GetUserByEmail(string emailAddress)
    {
        return await ctx.Users.FirstAsync(x => x.Email == emailAddress);
    }


    private string SHA512StringHash(string password)
    {
        return BitConverter.ToString(SHA512
                .Create()
                .ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
    
}