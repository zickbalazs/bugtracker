using System.ComponentModel.DataAnnotations;

namespace Bugtracker.Models.DTOs;

public class LoginForm
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
}