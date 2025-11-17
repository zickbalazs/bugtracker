using Bugtracker.Data;
using Bugtracker.Models;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using Microsoft.EntityFrameworkCore;

namespace Bugtracker.Main.Services;

public class DbPriorityService(BugtrackerContext ctx) : IPriorityService
{
    public async Task CreateAsync(PriorityDto newPriority)
    {
        await ctx.AddAsync(new Priority()
        {
            Title = newPriority.Title,
            ColorCode = newPriority.ColorCode ?? "#abc"
        });
        await ctx.SaveChangesAsync();
    }

    public Task<Priority> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<Priority>> GetAllAsync()
    {
        return await ctx.Priorities.Include(x=>x.Bugs).ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var priority = await ctx.Priorities.FindAsync(id);
        ctx.Priorities.Remove(priority ?? throw new KeyNotFoundException("priority with this id is not found"));
        await ctx.SaveChangesAsync();
    }

    public async Task ModifyAsync(int id, PriorityDto modificationData)
    {
        var priority = await ctx.Priorities.FindAsync(id) ?? throw new KeyNotFoundException("priority with this id is not found");
        priority.Title = modificationData.Title;
        priority.ColorCode = modificationData.ColorCode ?? priority.ColorCode;
        await ctx.SaveChangesAsync();
    }
}