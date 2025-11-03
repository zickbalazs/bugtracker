using Bugtracker.Data;
using Bugtracker.Models;
using Bugtracker.Services;
using Microsoft.EntityFrameworkCore;

namespace Bugtracker.Frontend.Services;

public class BugCommentService(BugtrackerContext ctx) : IBugCommentService
{
    public async Task DeleteAsync(int id)
    {
        await ctx.Comments
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task UpdateAsync(int id, string newContent)
    {
        await ctx.Comments
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(obj => 
                obj.SetProperty(x => x.Description, newContent));
    }

    public async Task CreateAsync(string content, User author, Bug parent)
    {
        await ctx.Comments.AddAsync(new BugComment
        {
            Author = author,
            Parent = parent,
            Description = content
        });
        await ctx.SaveChangesAsync();
    }

    public async Task<IList<BugComment>> GetByBugAsync(Bug bug)
    {
        return await ctx.Comments.Where(x => x.Parent == bug).ToListAsync();
    }

    public async Task<IList<BugComment>> GetByUser(User user)
    {
        return await ctx.Comments
            .Where(x => x.Author == user)
            .ToListAsync();
    }

    public async Task<BugComment> GetAsync(int id)
    {
        return await ctx.Comments.FindAsync(id) ?? throw new KeyNotFoundException();
    }
}