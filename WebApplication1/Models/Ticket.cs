using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;


public class Ticket
{
    [Key]
    public int TicketID { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;


    [Required]
    public int CreatedById { get; set; }


    [ForeignKey(nameof(CreatedById))]
    public Employee? CreatedBy { get; set; } 


    [Required]
    public StatusEnum Status { get; set; }


    [Required]
    public PriorityEnum Priority { get; set; }


    [Required]
    public bool IsResolved { get; set; }


    public Ticket() { }


    public Ticket(string title, string description, PriorityEnum priority, Employee createdBy, StatusEnum status = StatusEnum.Open, bool isResolved = false)
    {
        Title = title;
        Description = description;
        Priority = priority;
        CreatedBy = createdBy;
        CreatedById = createdBy.UserId;
        Status = status;
        IsResolved = isResolved;
    }

}
public enum StatusEnum
{
    Open,
    InProgress,
    Resolved,
    Closed
}

public enum PriorityEnum
{
    Low,
    Medium,
    High
}