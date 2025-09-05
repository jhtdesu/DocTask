using System.ComponentModel.DataAnnotations;

namespace DocTask.Core.Dtos.Auth;

public class RegisterRequest
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    public int? OrgId { get; set; } = null;
    public int? UnitId { get; set; } = null;
    public int? PositionId { get; set; } = null;
    public string? PositionName { get; set; } = null;
}
