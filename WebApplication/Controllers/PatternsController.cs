using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication;

namespace WebApplication.Controllers
{
    public class PatternsController : Controller
    {
        private readonly DBExcursionsContext _context;

        public PatternsController(DBExcursionsContext context)
        {
            _context = context;
        }

        // GET: Patterns
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pattern.ToListAsync());
        }

        // GET: Patterns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pattern = await _context.Pattern
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pattern == null)
            {
                return NotFound();
            }

            return View(pattern);
        }

        // GET: Patterns/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patterns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Duration,Description,MaxSize")] Pattern pattern)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pattern);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pattern);
        }

        // GET: Patterns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pattern = await _context.Pattern.FindAsync(id);
            if (pattern == null)
            {
                return NotFound();
            }
            return View(pattern);
        }

        // POST: Patterns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Duration,Description,MaxSize")] Pattern pattern)
        {
            if (id != pattern.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pattern);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatternExists(pattern.Id))
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
            return View(pattern);
        }

        // GET: Patterns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pattern = await _context.Pattern
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pattern == null)
            {
                return NotFound();
            }

            return View(pattern);
        }

        // POST: Patterns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pattern = await _context.Pattern.FindAsync(id);
            var excGuidesToDelete = from i in _context.ExcurionGuide
                                           where i.Idpattern == id
                                           select i;
            foreach (var i in excGuidesToDelete)
            {
                ExcurionGuidesController.Delete(i.Id, _context);
                _context.ExcurionGuide.Remove(i);
            }
            _context.Pattern.Remove(pattern);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PatternExists(int id)
        {
            return _context.Pattern.Any(e => e.Id == id);
        }
    }
}
