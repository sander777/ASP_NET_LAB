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
    public class ExcurionGuidesController : Controller
    {
        private readonly DBExcursionsContext _context;

        public ExcurionGuidesController(DBExcursionsContext context)
        {
            _context = context;
        }

        // GET: ExcurionGuides
        public async Task<IActionResult> Index()
        {
            var dBExcursionsContext = _context.ExcurionGuide.Include(e => e.IdguideNavigation).Include(e => e.IdpatternNavigation);
            return View(await dBExcursionsContext.ToListAsync());
        }

        // GET: ExcurionGuides/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var excurionGuide = await _context.ExcurionGuide
                .Include(e => e.IdguideNavigation)
                .Include(e => e.IdpatternNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (excurionGuide == null)
            {
                return NotFound();
            }

            return View(excurionGuide);
        }

        // GET: ExcurionGuides/Create
        public IActionResult Create()
        {
            ViewData["Idguide"] = new SelectList(_context.Guide, "Id", "FullName");
            ViewData["Idpattern"] = new SelectList(_context.Pattern, "Id", "Description");
            return View();
        }

        // POST: ExcurionGuides/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Idguide,Idpattern,Price")] ExcurionGuide excurionGuide)
        {
            if (ModelState.IsValid)
            {
                _context.Add(excurionGuide);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idguide"] = new SelectList(_context.Guide, "Id", "FullName", excurionGuide.Idguide);
            ViewData["Idpattern"] = new SelectList(_context.Pattern, "Id", "Description", excurionGuide.Idpattern);
            return View(excurionGuide);
        }

        // GET: ExcurionGuides/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var excurionGuide = await _context.ExcurionGuide.FindAsync(id);
            if (excurionGuide == null)
            {
                return NotFound();
            }
            ViewData["Idguide"] = new SelectList(_context.Guide, "Id", "FullName", excurionGuide.Idguide);
            ViewData["Idpattern"] = new SelectList(_context.Pattern, "Id", "Description", excurionGuide.Idpattern);
            return View(excurionGuide);
        }

        // POST: ExcurionGuides/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Idguide,Idpattern,Price")] ExcurionGuide excurionGuide)
        {
            if (id != excurionGuide.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(excurionGuide);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExcurionGuideExists(excurionGuide.Id))
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
            ViewData["Idguide"] = new SelectList(_context.Guide, "Id", "FullName", excurionGuide.Idguide);
            ViewData["Idpattern"] = new SelectList(_context.Pattern, "Id", "Description", excurionGuide.Idpattern);
            return View(excurionGuide);
        }

        // GET: ExcurionGuides/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var excurionGuide = await _context.ExcurionGuide
                .Include(e => e.IdguideNavigation)
                .Include(e => e.IdpatternNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (excurionGuide == null)
            {
                return NotFound();
            }

            return View(excurionGuide);
        }

        // POST: ExcurionGuides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var excurionGuide = await _context.ExcurionGuide.FindAsync(id);
            Delete(id, _context);
            await _context.SaveChangesAsync();
            _context.ExcurionGuide.Remove(excurionGuide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public static void Delete(int id, DBExcursionsContext _context)
        {
            var excToDelete = from i in _context.Excursion
                                     where i.IdexcursionGuide == id
                                     select i;
            foreach (var i in excToDelete)
            {
                _context.Excursion.Remove(i);
            }
        }

        private bool ExcurionGuideExists(int id)
        {
            return _context.ExcurionGuide.Any(e => e.Id == id);
        }
    }
}
