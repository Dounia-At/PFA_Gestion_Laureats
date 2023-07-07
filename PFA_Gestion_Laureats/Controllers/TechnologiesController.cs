using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PFA_Gestion_Laureats.Models;
using X.PagedList;

namespace PFA_Gestion_Laureats.Controllers
{
    public class TechnologiesController : Controller
    {
        private readonly MyContext _context;

        public TechnologiesController(MyContext context)
        {
            _context = context;
        }

        // GET: Technologies
        public async Task<IActionResult> Index(int? page)
        {
            if (HttpContext.Session.GetString("Role") == "AgentDirection")
            {
                ViewBag.role = "AgentDirection";
            }
            int pageSize = 10; // Number of cities to display per page
            int pageNumber = page ?? 1; // Default page number

            List<Technologie> technologies = _context.Technologie.AsNoTracking().ToList();

            var techno = technologies.ToPagedList(pageNumber, pageSize);
            return _context.Technologie != null ? 
                          View(techno) :
                          Problem("Entity set 'MyContext.Technologie'  is null.");
        }

       
        // GET: Technologies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Technologies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Technologie technologie)
        {
            if (ModelState.IsValid)
            {
                string NewName = Guid.NewGuid() + technologie.Pic.FileName;
                string PathFile = Path.Combine("wwwroot/assets/img/logo", NewName);

                using (FileStream stream = System.IO.File.Create(PathFile))
                {
                    technologie.Pic.CopyTo(stream);
                    technologie.Logo = NewName;
                }
                _context.Add(technologie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(technologie);
        }

        // GET: Technologies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Technologie == null)
            {
                return NotFound();
            }

            var technologie = await _context.Technologie.FindAsync(id);
            if (technologie == null)
            {
                return NotFound();
            }
            return View(technologie);
        }

        // POST: Technologies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Technologie technologie)
        {
            if (id != technologie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (technologie.Pic != null)
                    {
                        System.IO.File.Delete(Path.Combine("wwwroot/assets/img/logo", technologie.Logo));
                        string NewName = Guid.NewGuid() + technologie.Pic.FileName;
                        string PathFile = Path.Combine("wwwroot/assets/img/logo", NewName);

                        using (FileStream stream = System.IO.File.Create(PathFile))
                        {
                            technologie.Pic.CopyTo(stream);
                            technologie.Logo = NewName;
                        }
                    }
                    _context.Update(technologie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TechnologieExists(technologie.Id))
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
            return View(technologie);
        }

        // GET: Technologies/Delete/5
        public IActionResult Delete(int id)
        {
            if (_context.Technologie == null)
            {
                return Problem("Entity set 'MyContext.Technologie'  is null.");
            }
            Technologie technologie = _context.Technologie.Find(id);
            if (technologie != null)
            {
                _context.Technologie.Remove(technologie);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        
       

        private bool TechnologieExists(int id)
        {
          return (_context.Technologie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
