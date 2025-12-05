using Bogus;
using global::WebApplication1.Data;
using global::WebApplication1.Models;

namespace WebApplication1.Services;

public class TestUserService
{
    private readonly ApplicationDbContext _context;

    public TestUserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateEmployeeAsync(int count)
    {
        var faker = new Faker<Employee>()
            .CustomInstantiator(f => new Employee(
                f.Name.FullName(),
                f.Internet.Email(),
                f.Random.Bool()
            ));

        var employees = faker.Generate(count);

        _context.Employees.AddRange(employees);
        await _context.SaveChangesAsync();
    }

    public async Task CreateITSupportAsync(int count)
    {
        var faker = new Faker<ItSupport>()
            .CustomInstantiator(f => new ItSupport(
                f.Name.FullName(),
                f.Internet.Email(),
                f.Random.Bool(),
                f.PickRandom<Role>()
            ));

        var itSupports = faker.Generate(count);

        _context.ITSupports.AddRange(itSupports);
        await _context.SaveChangesAsync();
    }
}