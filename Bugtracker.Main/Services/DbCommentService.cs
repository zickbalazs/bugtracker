using Bugtracker.Data;
using Bugtracker.Models;
using Bugtracker.Services;
using Microsoft.EntityFrameworkCore;

namespace Bugtracker.Main.Services;

public class DbCommentService(BugtrackerContext ctx) : IBugCommentService
{
    public async Task DeleteAsync(int id)
    {
        var rows = await ctx.Comments.Where(x => x.Id == id).ExecuteDeleteAsync();
        if (rows < 1) throw new KeyNotFoundException();
    }

    public Task UpdateAsync(int id, string newContent)
    {
        throw new NotImplementedException();
    }

    public async Task CreateAsync(string content, User author, Bug parent)
    {
        var bugExists = await ctx.Bugs.FindAsync(parent.Id) != null;

        if (!bugExists) throw new KeyNotFoundException("The bug doesn't exist, it might have been removed");

        var authorExists = await ctx.Users.FindAsync(author.Id) != null;

        if (!authorExists) throw new KeyNotFoundException("This author doesn't exist");

        await ctx.Comments.AddAsync(new BugComment
        {
            Author = author,
            Description = content,
            Parent = parent
        });

        await ctx.SaveChangesAsync();
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