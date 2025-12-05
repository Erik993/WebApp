using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ItSupportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItSupportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ItSupports
        public async Task<IActionResult> Index()
        {
            return View(await _context.ITSupports.ToListAsync());
        }

        // GET: ItSupports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itSupport = await _context.ITSupports
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (itSupport == null)
            {
                return NotFound();
            }

            return View(itSupport);
        }

        // GET: ItSupports/Create
        public IActionResult Create()
        {
            //populate Roles with enums. Viewbag contains all enum values
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(Role)).Cast<Role>());

            return View();
        }

        // POST: ItSupports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Specialization,UserId,UserName,Email,IsActive")] ItSupport itSupport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itSupport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(itSupport);
        }

        // GET: ItSupports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itSupport = await _context.ITSupports.FindAsync(id);
            if (itSupport == null)
            {
                return NotFound();
            }

            //populate Roles with enums. Viewbag contains all enum values
            ViewBag.Roles = new SelectList(Enum.GetValues(typeof(Role)).Cast<Role>());
            return View(itSupport);
        }

        // POST: ItSupports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Specialization,UserId,UserName,Email,IsActive")] ItSupport itSupport)
        {
            if (id != itSupport.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itSupport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItSupportExists(itSupport.UserId))
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
            return View(itSupport);
        }

        // GET: ItSupports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itSupport = await _context.ITSupports
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (itSupport == null)
            {
                return NotFound();
            }

            return View(itSupport);
        }

        // POST: ItSupports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itSupport = await _context.ITSupports.FindAsync(id);
            if (itSupport != null)
            {
                _context.ITSupports.Remove(itSupport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItSupportExists(int id)
        {
            return _context.ITSupports.Any(e => e.UserId == id);
        }
    }
}
