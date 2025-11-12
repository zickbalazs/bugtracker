using Bugtracker.Data;
using Bugtracker.Models;
using Bugtracker.Models.DTOs;
using Bugtracker.Services;
using Microsoft.EntityFrameworkCore;

namespace Bugtracker.Main.Services;

public class DbPriorityService(BugtrackerContext ctx) : IPriorityService
{
    public Task CreateAsync(PriorityDto newPriority)
    {
        throw new NotImplementedException();
    }

    public Task<Priority> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<Priority>> GetAllAsync()
    {
        return await ctx.Priorities.ToListAsync();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task ModifyAsync(int id, PriorityDto modificationData)
    {
        throw new NotImplementedException();
    }
}