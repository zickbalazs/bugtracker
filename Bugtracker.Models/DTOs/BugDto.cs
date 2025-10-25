using System.ComponentModel.DataAnnotations;

namespace Bugtracker.Models.DTOs;

public class BugDto
{
    [Required]
    public required string Title { get; set; }
    [Required]
    public required string ShortDescription { get; set; }
    [Required]
    public required string Description { get; set; }
    [Required]
    public required Priority Priority { get; set; }
}