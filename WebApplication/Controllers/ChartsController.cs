using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : Controller
    {
        private readonly DBExcursionsContext _context;

        public ChartsController(DBExcursionsContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var pattern = _context.Pattern.ToList();

            List<object> excPattern = new List<object>();
            excPattern.Add(new[] { "Паттерн", "К-сть ексурсій" });

            foreach(var i in pattern)
            {
                var exGd = (from e in _context.ExcurionGuide
                                  where e.Idpattern == i.Id
                                  select e).ToList();
                int sum = 0;
                foreach(var j in exGd)
                {
                    sum += (from ex in _context.Excursion
                            where ex.IdexcursionGuide == j.Id
                            select ex).Count();
                }
                excPattern.Add(new object[] { i.Name, sum });
            }
            return new JsonResult(excPattern);
        }

        public string test()
        {
            return "121212";
        }
    }
}