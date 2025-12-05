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
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tickets.Include(t => t.CreatedBy);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(m => m.TicketID == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        //in _loginPartial there are alert logic
        [Authorize]
        public IActionResult Create()
        {
            ViewData["CreatedById"] = new SelectList(_context.Employees, "UserId", "UserName");

            //populate with statuses and priorities
            ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>());
            ViewBag.Priorities = new SelectList(Enum.GetValues(typeof(PriorityEnum)).Cast<PriorityEnum>());
            
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketID,Title,Description,CreatedById,Status,Priority,IsResolved")] Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                //to debug, show errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                foreach (var error in errors)
                    Debug.WriteLine(error);

                // populate fields
                ViewData["CreatedById"] = new SelectList(_context.Employees, "UserId", "UserName", ticket.CreatedById);
                ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>());
                ViewBag.Priorities = new SelectList(Enum.GetValues(typeof(PriorityEnum)).Cast<PriorityEnum>());
                return View(ticket);
            }

            // load Employee entity for navigation property
            //ticket.CreatedBy = await _context.Employees.FindAsync(ticket.CreatedById);

            _context.Add(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["CreatedById"] = new SelectList(_context.Employees, "UserId", "UserName", ticket.CreatedById);
            ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>());
            ViewBag.Priorities = new SelectList(Enum.GetValues(typeof(PriorityEnum)).Cast<PriorityEnum>());

            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketID,Title,Description,CreatedById,Status,Priority,IsResolved")] Ticket ticket)
        {
            if (id != ticket.TicketID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.TicketID))
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
            ViewData["CreatedById"] = new SelectList(_context.Employees, "UserId", "UserName", ticket.CreatedById);
            ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>());
            ViewBag.Priorities = new SelectList(Enum.GetValues(typeof(PriorityEnum)).Cast<PriorityEnum>());

            return View(ticket);
        }

        // GET: Tickets/Delete/5
        //flag
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(m => m.TicketID == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketID == id);
        }
    }
}
