using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

public class TestDataController : Controller
{
    private readonly TestUserService _testUserService;
    private readonly TestTicketService _testTicketService;
    private readonly TestAssignmentService _testAssignmentService;

    public TestDataController(TestUserService testUserService, TestTicketService testTicketService,
        TestAssignmentService testAssignmentService)
    {
        _testUserService = testUserService;
        _testTicketService = testTicketService;
        _testAssignmentService = testAssignmentService;
    }

    public async Task<IActionResult> CreateEmployees(int count)
    {
        await _testUserService.CreateEmployeeAsync(count);

        //show as alert under the navbar. code in _layout.cshtml
        TempData["Success"] = $"{count} test employees created.";
        return RedirectToAction("Index", "Employees");
    }


    public async Task<IActionResult> CreateITSupports(int count)
    {
        await _testUserService.CreateITSupportAsync(count);

        //show as alert under the navbar. code in _layout.cshtml
        TempData["Success"] = $"{count} test it supports created.";
        return RedirectToAction("Index", "ItSupports");
    }



    public async Task<IActionResult> CreateTickets(int count)
    {
        //bool - true if at least one employee exists
        bool ok = await _testTicketService.CreateTicketAsync(count);

        if (!ok)
        {
            //show as alert under the navbar. code in _layout.cshtml
            TempData["Error"] = "Cannot create tickets — no employees exist.Create at least one";
            return RedirectToAction("Index", "Employees");
        }

        //show as alert under the navbar. code in _layout.cshtml
        TempData["Success"] = $"{count} test tickets created.";
        return RedirectToAction("Index", "Tickets");
    }


    public async Task<IActionResult> CreateTestAssignments(int count)
    {
        bool ok = await _testAssignmentService.CreateAssignmentAsync(count);

        if(!ok)
        {
            //show as alert under the navbar. code in _layout.cshtml
            TempData["Error"] = "Cannot create assignment — no it support or ticket exist.Create at least one it support and one ticket";
            return RedirectToAction("Index", "ItSupports");
        }

        //show as alert under the navbar. code in _layout.cshtml
        TempData["Success"] = $"{count} test assignments created.";
        return RedirectToAction("Index", "Assignments");
    }
}
