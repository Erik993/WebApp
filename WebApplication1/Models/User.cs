using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public abstract class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; }

    public User() { }

    public User(string name, string email, bool isactive)
    {
        UserName = name;
        Email = email;
        IsActive = isactive;
    }
}