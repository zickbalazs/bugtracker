using Bugtracker.Models;
using Microsoft.EntityFrameworkCore;

namespace Bugtracker.Data;

public class BugtrackerContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } 
    
    public DbSet<Bug> Bugs { get; set; }
    
    public DbSet<BugComment> Comments { get; set; }
    
    public DbSet<Priority> Priorities { get; set; }
}