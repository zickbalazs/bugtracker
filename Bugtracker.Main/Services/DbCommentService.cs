using Bugtracker.Data;
using Bugtracker.Models;
using Bugtracker.Services;
using Microsoft.EntityFrameworkCore;

namespace Bugtracker.Main.Services;

public class DbCommentService(BugtrackerContext ctx) : IBugCommentService
{
    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(int id, string newContent)
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync(string content, User author, Bug parent)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<BugComment>> GetByBugAsync(Bug bug)
    {
        return await ctx.Comments
            .Include(x => x.Parent)
            .Include(x=>x.Author)
            .Where(x => x.Parent.Id == bug.Id)
            .ToListAsync();
    }

    public Task<IList<BugComment>> GetByUser(User user)
    {
        throw new NotImplementedException();
    }

    public Task<BugComment> GetAsync(int id)
    {
        throw new NotImplementedException();
    }
}