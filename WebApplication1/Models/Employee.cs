using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class Employee : User
{
    //Contract date cannot be changed after initialization
    [Required]
    public DateTime ContractDate { get; set; }

    //constructor has 1 optional argument - contract date, by default, if it is not provided it is null.
    //If null, then assign DateTime.Now date.
    public Employee(string name, string email, bool isactive, DateTime? contractDate = null) : base(name, email, isactive)
    {
        ContractDate = contractDate ?? DateTime.Now;
    }

    public Employee() { }
}