using Bugtracker.Models;
using Bugtracker.Models.DTOs;

namespace Bugtracker.Services;

public interface IBugService
{
    public Task CreateAsync(BugDto newBug, int userId);
    
    public Task DeleteAsync(int id);
    
    public Task UpdateAsync(BugDto updateData, int id);
    
    public Task<List<Bug>> GetAllAsync();
    
    public Task<List<Bug>> GetUnsolvedAsync();

    public Task<Bug> GetAsync(int id);
}