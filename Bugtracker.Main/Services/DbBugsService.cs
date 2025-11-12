using Bugtracker.Data;
using Bugtracker.Models;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using Microsoft.EntityFrameworkCore;

namespace Bugtracker.Main.Services;

public class DbBugsService(BugtrackerContext ctx) : IBugService
{
    public async Task CreateAsync(BugDto newBug, int userId)
    {
        var user = await ctx.Users.FindAsync(userId) ?? throw new KeyNotFoundException();


        var toBeAddedEntry = new Bug
        {
            Title = newBug.Title,
            Description = newBug.Description,
            ShortDescription = newBug.ShortDescription,
            Solved = false,
            Priority = newBug.Priority,
            Author = user
        };

        await ctx.Bugs.AddAsync(toBeAddedEntry);
        await ctx.SaveChangesAsync();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(BugDto updateData, int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Bug>> GetAllAsync()
    {
        return await ctx.Bugs
            .Include(x=>x.Comments)
            .Include(x=>x.Author)
            .Include(x=>x.Priority)
            .ToListAsync();
    }

    public async Task<List<Bug>> GetUnsolvedAsync()
    {
        return (await GetAllAsync()).Where(x => !x.Solved).ToList();
    }

    public async Task<Bug> GetAsync(int id)
    {
        return await ctx.Bugs.FindAsync(id) ?? throw new KeyNotFoundException("Bug with this id is not found");
    }
}