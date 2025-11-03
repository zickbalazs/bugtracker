using Bugtracker.Models;
using Bugtracker.Models.DTOs;

namespace Bugtracker.Services;

public interface IBugService
{
    public Task CreateAsync(BugDto newBug);
    
    public Task DeleteAsync(int id);
    
    public Task UpdateAsync(BugDto updateData);
    
    public Task<IList<Bug>> GetAllAsync();
    
    public Task<IList<Bug>> GetUnsolvedAsync();

    public Task<Bug> GetAsync(int id);
}