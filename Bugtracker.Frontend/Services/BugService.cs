using Bugtracker.Data;
using Bugtracker.Models;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using Microsoft.EntityFrameworkCore;
using UIKit;

namespace Bugtracker.Frontend.Services;

public class BugService(BugtrackerContext ctx) : IBugService
{
    public async Task CreateAsync(BugDto newBug, int userId)
    {
        Bug newEntry = new Bug
        {
            Title = newBug.Title,
            ShortDescription = newBug.ShortDescription,
            Description = newBug.Description,
            Author = await ctx.Users.FindAsync(userId) ?? throw new KeyNotFoundException()
        };
        await ctx.Bugs.AddAsync(newEntry);
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var bug = await ctx.Bugs.FirstOrDefaultAsync(x => x.Id == id);

        if (bug == null)
            throw new KeyNotFoundException();

        await ctx.Bugs.Where(x => x == bug).ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(BugDto updateData, int id)
    {
        await ctx.Bugs
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(opt =>
                 opt.SetProperty(x => x.Description, updateData.Description)
                    .SetProperty(x => x.ShortDescription, updateData.ShortDescription)
                    .SetProperty(x => x.Title, updateData.Title));
    }

    public async Task<IList<Bug>> GetAllAsync()
    {
        return await ctx.Bugs.ToListAsync();
    }

    public async Task<IList<Bug>> GetUnsolvedAsync()
    {
        return await ctx.Bugs.Where(x => !x.Solved).ToListAsync();
    }

    public async Task<Bug> GetAsync(int id)
    {
        return await ctx.Bugs.FindAsync(id) ?? throw new KeyNotFoundException();
    }
}