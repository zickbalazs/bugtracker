using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bugtracker.Models;

public class Priority
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public required string Title { get; set; }

    public string ColorCode { get; set; } = "#fff";

    public virtual IList<Bug> Bugs { get; } = [];
}