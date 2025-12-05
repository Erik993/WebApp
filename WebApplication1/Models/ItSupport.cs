using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class ItSupport : User
{
    [Required]
    public Role Specialization { get; set; }

    public ItSupport() { }

    public ItSupport(string name, string email, bool isactive, Role spec) : base(name, email, isactive)
    {
        Specialization = spec;
    }
}

public enum Role
{
    Network,
    Software,
    Hardware,
    Security,
    HelpDesk
}
