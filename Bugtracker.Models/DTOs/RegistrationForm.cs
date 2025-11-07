using System.ComponentModel.DataAnnotations;

namespace Bugtracker.Models.DTOs;

public class RegistrationForm
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string Password { get; set; }
}