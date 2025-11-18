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
            AssociatedFileName = newBug.FileName,
            Author = user
        };

        await ctx.Bugs.AddAsync(toBeAddedEntry);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var bug = await ctx.Bugs.FindAsync(id);
        ctx.Bugs.Remove(bug ?? throw new KeyNotFoundException("bug with this id is not found"));
        await ctx.SaveChangesAsync();
    }

    public async Task UpdateAsync(BugDto updateData, int id)
    {
        var bugToEdit = await ctx.Bugs.FindAsync(id);

        if (bugToEdit == null)
            throw new KeyNotFoundException("bug with this id is not found!");

        bugToEdit.Title = updateData.Title;
        bugToEdit.ShortDescription = updateData.ShortDescription;
        bugToEdit.Description = updateData.Description;
        bugToEdit.Priority = updateData.Priority;

        await ctx.SaveChangesAsync();
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
        var bug = await ctx.Bugs.FindAsync(id) ?? throw new KeyNotFoundException("bug with this id is not found");
        bug.Solved = true;
        bug.SolvedOn = DateTime.UtcNow;
        await ctx.SaveChangesAsync();
    }
}