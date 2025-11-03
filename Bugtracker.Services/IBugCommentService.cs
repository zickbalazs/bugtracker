using Bugtracker.Models;

namespace Bugtracker.Services;

public interface IBugCommentService
{
    public Task DeleteAsync(int id);

    public Task UpdateAsync(int id, string newContent);

    public Task CreateAsync(string content, User author, Bug parent);

    public Task<IList<BugComment>> GetByBugAsync(Bug bug);

    public Task<IList<BugComment>> GetByUser(User user);
    
    public Task<BugComment> GetAsync(int id);
}