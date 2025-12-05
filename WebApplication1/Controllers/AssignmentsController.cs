using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Assignments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Assignments.Include(a => a.ItSupport).Include(a => a.Ticket);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Assignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments
                .Include(a => a.ItSupport)
                .Include(a => a.Ticket)
                .FirstOrDefaultAsync(m => m.AssignmentId == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // GET: Assignments/Create
        //in _loginPartial there are alert logic
        [Authorize]
        public IActionResult Create()
        {
            ViewData["ItSupportId"] = new SelectList(_context.ITSupports, "UserId", "Email");
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketID", "Title");
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssignmentId,AssignedAt,ItSupportId,TicketId,Comment")] Assignment assignment)
        {

            if (!ModelState.IsValid)
            {
                //to debug, show errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                    Debug.WriteLine(error);

                ViewData["ItSupportId"] = new SelectList(_context.ITSupports, "UserId", "UserName", assignment.ItSupportId);
                ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketID", "Title", assignment.TicketId);
                return View(assignment);
            }

                _context.Add(assignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

        }

        // GET: Assignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }
            ViewData["ItSupportId"] = new SelectList(_context.ITSupports, "UserId", "UserName", assignment.ItSupportId);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketID", "Title", assignment.TicketId);
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AssignmentId,AssignedAt,ItSupportId,TicketId,Comment")] Assignment assignment)
        {
            if (id != assignment.AssignmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentExists(assignment.AssignmentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItSupportId"] = new SelectList(_context.ITSupports, "UserId", "UserName", assignment.ItSupportId);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketID", "Title", assignment.TicketId);
            return View(assignment);
        }

        // GET: Assignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments
                .Include(a => a.ItSupport)
                .Include(a => a.Ticket)
                .FirstOrDefaultAsync(m => m.AssignmentId == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssignmentExists(int id)
        {
            return _context.Assignments.Any(e => e.AssignmentId == id);
        }
    }
}
