using System.ComponentModel.DataAnnotations;

namespace Bugtracker.Models.DTOs;

public class PriorityDto
{
    [Required]
    public required string Title { get; set; }
    public string? ColorCode { get; set; } = null;
}