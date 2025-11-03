using Bugtracker.Data;
using Bugtracker.Models;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using Microsoft.EntityFrameworkCore;

namespace Bugtracker.Frontend.Services;

public class PriorityService(BugtrackerContext ctx) : IPriorityService
{
    public async Task CreateAsync(PriorityDto newPriority)
    {
        await ctx.Priorities.AddAsync(new Priority
        {
            Title = newPriority.Title,
            ColorCode = newPriority.ColorCode ?? "#fff"
        });
        await ctx.SaveChangesAsync();
    }

    public async Task<Priority> GetAsync(int id)
    {
        return await ctx.Priorities.FindAsync(id) ?? throw new KeyNotFoundException();
    }

    public async Task<IList<Priority>> GetAllAsync()
    {
        return await ctx.Priorities.ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await ctx.Priorities.Where(x => x.Id == id).ExecuteDeleteAsync();
    }

    public async Task ModifyAsync(int id, PriorityDto modificationData)
    {
        await ctx.Priorities
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(obj =>
                obj.SetProperty(x => x.Title, modificationData.Title)
                    .SetProperty(x => x.ColorCode, modificationData.ColorCode));
    }
}