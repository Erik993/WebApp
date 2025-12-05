using Bogus;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services;
public class TestTicketService
{
    private readonly ApplicationDbContext _context;

    public TestTicketService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateTicketAsync(int count)
    {
        // Check if there are employees
        var employees = await _context.Employees.ToListAsync();
        if (!employees.Any())
        {
            return false; 
        }

        // create new tickets
        var faker = new Faker<Ticket>()
            .CustomInstantiator(f =>
            {
                var employee = f.PickRandom(employees);

                return new Ticket(
                    title: f.Lorem.Sentence(3),
                    description: f.Lorem.Sentence(30),
                    priority: f.PickRandom<PriorityEnum>(),
                    createdBy: employee,
                    status: f.PickRandom<StatusEnum>(),
                    isResolved: false
                );
            });

        var items = faker.Generate(count);

        // saving
        await _context.Tickets.AddRangeAsync(items);
        await _context.SaveChangesAsync();

        return true;
    }
}
