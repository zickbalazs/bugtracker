using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bugtracker.Models;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public bool IsAdmin { get; set; } = false;
    [MinLength(3)]
    public string Name { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }

    public virtual IList<Bug> Bugs { get; } = [];
    public virtual IList<BugComment> Comments { get; } = [];
}