using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

public class Assignment
{
    [Key]
    public int AssignmentId { get; set; }
    public DateTime AssignedAt { get; set; } = DateTime.Now;


    public int ItSupportId { get; set; }

    [ForeignKey(nameof(ItSupportId))]
    public ItSupport? ItSupport { get; set; }


    public int TicketId { get; set; }

    [ForeignKey(nameof(TicketId))]
    public Ticket? Ticket { get; set; }


    [MaxLength(200)]
    public string Comment { get; set; } = string.Empty;


    public Assignment() { }

    public Assignment(ItSupport support, Ticket ticket, string comment = "")
    {
        //AssignedAt = DateTime.Now;
        ItSupport = support;
        ItSupportId = support.UserId;
        Ticket = ticket;
        TicketId = ticket.TicketID;
        Comment = comment;
    }
}
