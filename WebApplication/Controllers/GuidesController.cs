using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication;
using WebApplication.Models;
using ClosedXML.Excel;
using System.IO;

namespace WebApplication.Controllers
{
    public class GuidesController : Controller
    {
        private readonly DBExcursionsContext _context;

        public GuidesController(DBExcursionsContext context)
        {
            _context = context;
        }

        // GET: Guides
        public async Task<IActionResult> Index()
        {
            var dBExcursionsContext = _context.Guide.Include(g => g.Category);
            return View(await dBExcursionsContext.ToListAsync());
        }

        // GET: Guides/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guide = await _context.Guide
                .Include(g => g.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (guide == null)
            {
                return NotFound();
            }

            return View(guide);
        }

        // GET: Guides/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            ViewBag.Patterns = _context.Pattern.ToList();
            return View();
        }

        // POST: Guides/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,CategoryId")] Guide guide)
        {
            System.Diagnostics.Debug.WriteLine(Request.Form["FullName"].ToString());
            if (ModelState.IsValid)
            {
                _context.Add(guide);
                await _context.SaveChangesAsync();
                foreach (var i in _context.Pattern)
                {
                    string selected = Request.Form[i.Id.ToString()];
                    string price = Request.Form[i.Id.ToString() + "-price"];
                    System.Diagnostics.Debug.WriteLine(i.Name + " " + selected + " " + price);

                    if (selected != "on") continue;

                    ExcurionGuide add = new ExcurionGuide
                    {
                        Price = Convert.ToDecimal(price),
                        Idguide = guide.Id,
                        Idpattern = i.Id,
                    };
                   if ((from e_g in _context.ExcurionGuide
                        where e_g.Idguide == add.Idguide && e_g.Idpattern == add.Idpattern && e_g.Price == add.Price
                        select e_g).ToList().Count() == 0)
                    {
                        _context.Add(add);
                    }
                    
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", guide.CategoryId);
            return View(guide);
        }
        

        // GET: Guides/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var guide = await _context.Guide.FindAsync(id);
            if (guide == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", guide.CategoryId);
            ViewBag.Patterns = _context.Pattern.ToList();
            var l = (from p in _context.Pattern
                    select new box { pattern = p, price = Convert.ToDecimal(0.0), check = false }).ToList();
            foreach (var i in (from p in _context.ExcurionGuide
                               where p.Idguide == guide.Id
                               select p))
            {
                for(int j = 0; j < l.Count(); j++)
                {
                    if (l[j].pattern.Id == i.Idpattern)
                    {
                        l[j].price = i.Price;
                        l[j].check = true;
                    }
                }
            }
            ViewBag.ExcursionGuides = l;
            return View(guide);
        }

        // POST: Guides/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,CategoryId")] Guide guide)
        {
            if (id != guide.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(guide);
                    await _context.SaveChangesAsync();
                    foreach (var i in _context.Pattern)
                    {
                        string selected = Request.Form[i.Id.ToString()];
                        string price = Request.Form[i.Id.ToString() + "-price"];
                        System.Diagnostics.Debug.WriteLine(i.Name + " " + selected + " " + price);

                        if (selected != "on")
                        {
                            var ep = (from eg in _context.ExcurionGuide
                                      where eg.Idguide == id && eg.Idpattern == i.Id
                                      select eg).ToList();
                            if(ep.Count > 0)
                            {
                                foreach(var e in ep)
                                {
                                    _context.Remove(e);
                                }
                            }
                            continue;
                        }

                        ExcurionGuide add = new ExcurionGuide
                        {
                            Price = Convert.ToDecimal(price),
                            Idguide = guide.Id,
                            Idpattern = i.Id,
                        };
       
                        _context.Add(add);
                      

                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GuideExists(guide.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", guide.CategoryId);
            return View(guide);
        }

        // GET: Guides/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var guide = await _context.Guide
                .Include(g => g.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (guide == null)
            {
                return NotFound();
            }

            return View(guide);
        }

        // POST: Guides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var guide = await _context.Guide.FindAsync(id);
            var excGuidesToDelete = (from i in _context.ExcurionGuide
                                     where i.Idguide == id
                                     select i).ToList();
            foreach (var i in excGuidesToDelete)
            {
                ExcurionGuidesController.Delete(i.Id, _context);
                _context.ExcurionGuide.Remove(i);
            }
            await _context.SaveChangesAsync();
            _context.Guide.Remove(guide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GuideExists(int id)
        {
            return _context.Guide.Any(e => e.Id == id);
        }

        public ActionResult Export()
        {
            DateTime date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var guides = _context.Guide.ToList();

                foreach(var g in guides)
                {
                    var worksheet = workbook.Worksheets.Add(g.FullName + "__" + g.Id);
                    var exc = (from e in _context.Excursion
                               join eg in _context.ExcurionGuide on e.IdexcursionGuide equals eg.Id
                               where eg.Idguide == g.Id && e.Date > date && e.Date < DateTime.Now
                               select new { Date = e.Date, Price = eg.Price }).ToList();
                    int i = 1;
                    worksheet.Column(1).Width *= 3;
                    foreach(var e in exc)
                    {
                        worksheet.Cell(i, 1).Value = e.Date.ToString();
                        worksheet.Cell(i, 2).Value = e.Price;

                        i++;
                    }
                    worksheet.Cell(i, 1).Value = "Всього";
                    string form = "=SUM(B1:B" + (i - 1) + ")";
                    worksheet.Cell(i, 2).FormulaA1 = form;

                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }

            }

        }

    }
}
