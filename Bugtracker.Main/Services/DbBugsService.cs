using System.Xml.Schema;
using Bugtracker.Data;
using Bugtracker.Models;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using CommunityToolkit.Mvvm.Messaging;
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

    public async Task DeleteAsync(int id)
    {
        await ctx.Bugs.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(BugDto updateData, int id)
    {
        await ctx.Bugs.Where(x => x.Id == id).ExecuteUpdateAsync(prop =>
            prop.SetProperty(x => x.Description, updateData.Description)
                .SetProperty(x => x.ShortDescription, updateData.ShortDescription)
                .SetProperty(x => x.Title, updateData.Title)
                .SetProperty(x => x.Priority, updateData.Priority));
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

    public async Task MarkBugAsSolved(int id)
    {
        var rowsAffected = await ctx.Bugs.Where(x => x.Id == id)
            .ExecuteUpdateAsync(obj=>
                obj.SetProperty(x=>x.Solved, true)
                   .SetProperty(x=>x.SolvedOn, DateTime.UtcNow));

        if (rowsAffected < 1)
            throw new KeyNotFoundException();
    }
}