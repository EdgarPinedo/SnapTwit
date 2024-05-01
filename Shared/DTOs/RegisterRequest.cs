using System.ComponentModel.DataAnnotations;

namespace Snaptwit.Shared.DTOs;

public class RegisterRequest
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^[0-9]+$", ErrorMessage = "That is not a valid phone number")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must contain 10 digits")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(15, MinimumLength = 6, ErrorMessage = "The password must contain between 6 and 15 characters")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(15, MinimumLength = 6, ErrorMessage = "The password must contain between 6 and 15 characters")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}