using Bogus;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services;
public class TestAssignmentService
{
    private readonly ApplicationDbContext _context;

    public TestAssignmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAssignmentAsync(int count)
    {
        //extract itsupports from context and check if at least 1 element exists
        //return false if no elements, otherwise will be returned true with new objects
        var itsupports = await _context.ITSupports.ToListAsync();
        if(!itsupports.Any())
        {
            return false;
        }

        //extract tickets from context and check if at least 1 element exists
        //return false if no elements, otherwise will be returned true with new objects
        var tickets = await _context.Tickets.ToListAsync();
        if(!tickets.Any())
        {
            return false;
        }

        var faker = new Faker<Assignment>()
            .CustomInstantiator(f =>
            {
                var itsupport = f.PickRandom(itsupports);
                var tckt = f.PickRandom(tickets);

                return new Assignment(
                    support: itsupport,
                    ticket: tckt,
                    comment: f.Lorem.Sentence(2));
            });

        var items = faker.Generate(count);

        await _context.Assignments.AddRangeAsync(items);
        await _context.SaveChangesAsync();

        return true;

    }
}


