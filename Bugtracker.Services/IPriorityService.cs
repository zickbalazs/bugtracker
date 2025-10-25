using Bugtracker.Models;
using Bugtracker.Models.DTOs;

namespace Bugtracker.Services;

public interface IPriorityService
{
    public Task CreateAsync(PriorityDto newPriority);
    
    public Task<Priority> GetAsync(int id);
    
    public Task<IList<Priority>> GetAllAsync();
    
    public Task DeleteAsync(int id);
    
    public Task ModifyAsync(int id, PriorityDto modificationData);
}