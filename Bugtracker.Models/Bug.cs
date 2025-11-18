using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bugtracker.Models;
/// <summary>
/// Represents an entity in the database.
/// </summary>
public class Bug
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public required string Title { get; set; }
    [Required]
    public required string ShortDescription { get; set; }
    [Required]
    public required string Description { get; set; }
    
    public string? AssociatedFileName { get; set; }
    
    public bool Solved { get; set; }
    public DateTime Created { get; set;  } = DateTime.UtcNow;
    public DateTime? SolvedOn { get; set; } = null;

    [Required]
    public virtual Priority Priority { get; set; } = null;
    [Required]
    public virtual User Author { get; set; } = null;
    
    public virtual IList<BugComment> Comments { get; } = [];
}