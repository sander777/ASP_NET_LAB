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
    public class ExcursionsController : Controller
    {
        private readonly DBExcursionsContext _context;

        public ExcursionsController(DBExcursionsContext context)
        {
            _context = context;
        }

        // GET: Excursions
        public async Task<IActionResult> Index()
        {
            var dBExcursionsContext = _context.Excursion.Include(e => e.IdexcursionGuideNavigation);
            ViewBag.exGdPt = from i in _context.Excursion
                             select new ExGdPtBox { ex = i, FullName = i.IdexcursionGuideNavigation.IdguideNavigation.FullName, Name = i.IdexcursionGuideNavigation.IdpatternNavigation.Name, Price = i.IdexcursionGuideNavigation.Price };
            return View(await dBExcursionsContext.ToListAsync());
        }

        // GET: Excursions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var excursion = await _context.Excursion
                .Include(e => e.IdexcursionGuideNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (excursion == null)
            {
                return NotFound();
            }

            return View(excursion);
        }

        // GET: Excursions/Create
        public IActionResult Create()
        {
            ViewData["IdexcursionGuide"] = new SelectList(_context.ExcurionGuide, "Id", "IdguideNavigation");
            ViewBag.ExcurionGuides = from i in _context.ExcurionGuide
                                     orderby i.IdguideNavigation.FullName
                                     select new ExGdBox{ ed = i, FullName = i.IdguideNavigation.FullName, Name = i.IdpatternNavigation.Name };
            return View();
        }

        // POST: Excursions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date")] Excursion excursion)
        {
            System.Diagnostics.Debug.WriteLine(Request.Form["ExGd"].ToString());
            excursion.IdexcursionGuide = Convert.ToInt32(Request.Form["ExGd"]);
            if (excursion.Date < DateTime.Now)
            {
                ViewBag.ExcurionGuides = from i in _context.ExcurionGuide
                                         select new ExGdBox { ed = i, FullName = i.IdguideNavigation.FullName, Name = i.IdpatternNavigation.Name };
                ModelState.AddModelError("Date", "Дата не може бути минулою");
            }

            foreach(var i in _context.Excursion)
            {
                var startTime = i.Date;
                var endTime = startTime +_context.Pattern.Find(_context.ExcurionGuide.Find(i.IdexcursionGuide).Idpattern).Duration;
                var iGide = _context.Guide.Find(_context.ExcurionGuide.Find(i.IdexcursionGuide).Idguide).Id;
                var eGide = _context.Guide.Find(_context.ExcurionGuide.Find(excursion.IdexcursionGuide).Idguide).Id;

                if (iGide == eGide && startTime < excursion.Date && endTime > excursion.Date)
                {
                    ViewBag.ExcurionGuides = from j in _context.ExcurionGuide
                                             select new ExGdBox { ed = j, FullName = j.IdguideNavigation.FullName, Name = j.IdpatternNavigation.Name };
                    ModelState.AddModelError("Date", "Цей гід вже проводить екскурсію в цей час");
                    break;
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(excursion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdexcursionGuide"] = new SelectList(_context.ExcurionGuide, "Id", "Id", excursion.IdexcursionGuide);
            return View(excursion);
        }

        // GET: Excursions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var excursion = await _context.Excursion.FindAsync(id);
            if (excursion == null)
            {
                return NotFound();
            }
            ViewBag.ExcurionGuides = from i in _context.ExcurionGuide
                                     orderby i.IdguideNavigation.FullName
                                     select new ExGdBox { ed = i, FullName = i.IdguideNavigation.FullName, Name = i.IdpatternNavigation.Name };
            ViewData["IdexcursionGuide"] = new SelectList(_context.ExcurionGuide, "Id", "Id", excursion.IdexcursionGuide);
            return View(excursion);
        }

        // POST: Excursions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date")] Excursion excursion)
        {
            if (id != excursion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    excursion.IdexcursionGuide = Convert.ToInt32(Request.Form["ExGd"]);
                    _context.Update(excursion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExcursionExists(excursion.Id))
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
            ViewData["IdexcursionGuide"] = new SelectList(_context.ExcurionGuide, "Id", "Id", excursion.IdexcursionGuide);
            return View(excursion);
        }

        // GET: Excursions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var excursion = await _context.Excursion
                .Include(e => e.IdexcursionGuideNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (excursion == null)
            {
                return NotFound();
            }

            return View(excursion);
        }

        // POST: Excursions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var excursion = await _context.Excursion.FindAsync(id);
            _context.Excursion.Remove(excursion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExcursionExists(int id)
        {
            return _context.Excursion.Any(e => e.Id == id);
        }
    }
}
