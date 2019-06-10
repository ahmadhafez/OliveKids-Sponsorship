using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OliveKids.Models;
using OliveKids.Repository;

namespace OliveKids.Controllers
{
    public class KidsController : Controller
    {
        private readonly OkSposershipContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public KidsController(OkSposershipContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions)
        {
            var kids = _context.Kids
                .Where(p => p.Sponsor == null)
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    i.Age,
                    i.Description
                });
            return Json(DataSourceLoader.Load(kids, loadOptions));
        }

        // GET: Kids
        public async Task<IActionResult> Index()
        {
            return View(await _context.Kids.ToListAsync());
        }

        // GET: Kids/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kid = await _context.Kids
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kid == null)
            {
                return NotFound();
            }

            return View(kid);
        }

        // GET: Kids/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kids/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DateOfBirth,Description")] Kid kid)
        {
            if (ModelState.IsValid)
            {
                kid.Id = Guid.NewGuid();
                _context.Add(kid);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kid);
        }

        // GET: Kids/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kid = await _context.Kids.FindAsync(id);
            if (kid == null)
            {
                return NotFound();
            }
            return View(kid);
        }

        // POST: Kids/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,DateOfBirth,Description")] Kid kid)
        {
            if (id != kid.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kid);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KidExists(kid.Id))
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
            return View(kid);
        }

        // GET: Kids/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kid = await _context.Kids
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kid == null)
            {
                return NotFound();
            }

            return View(kid);
        }

        // POST: Kids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var kid = await _context.Kids.FindAsync(id);
            _context.Kids.Remove(kid);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public ActionResult FileSelection(string name, string arabicName, DateTime dateOfBirth, string description, IFormFile photo)
        {

            if (photo != null)
            {
                Kid kid = new Kid()
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    ArabicName = arabicName,
                    DateOfBirth = dateOfBirth,
                    Description = description
                };

                SaveFile(photo, kid.Id);

                _context.Kids.Add(kid);
                _context.SaveChanges();
                ViewBag.KidName = kid.Name;

                return View("SubmissionSuccessful");
            }

            return View("SubmissionFailed");
        }

        void SaveFile(IFormFile file, Guid kidGuid)
        {
            try
            {
                var path = Path.Combine(_hostingEnvironment.WebRootPath, "kids");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string photoName = string.Format("{0}.png", kidGuid.ToString());
                using (var fileStream = System.IO.File.Create(Path.Combine(path, photoName)))
                {
                    file.CopyTo(fileStream);
                }
            }
            catch
            {
                Response.StatusCode = 400;
            }
        }
        private bool KidExists(Guid id)
        {
            return _context.Kids.Any(e => e.Id == id);
        }
    }
}
