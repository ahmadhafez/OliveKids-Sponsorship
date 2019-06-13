using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OliveKids.Logic;
using OliveKids.Models;
using OliveKids.Repository;
using MailKit.Net.Smtp;
using MimeKit;

namespace OliveKids.Controllers
{
    public class SponsorsController : Controller
    {
        private readonly OkSposershipContext _context;

        public SponsorsController(OkSposershipContext context)
        {
            _context = context;
        }

        // GET: Sponsors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sponsors.ToListAsync());
        }

        // GET: Sponsors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await _context.Sponsors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sponsor == null)
            {
                return NotFound();
            }

            return View(sponsor);
        }

        // GET: Sponsors/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([ModelBinder (binderType: typeof(SponsorModelBinder))] Sponsor sponsor)
        {
            List<Kid> kids = new List<Kid>();
            foreach (var vKid in sponsor.SponsoredKids) {
                var kid = _context.Kids.Find(vKid.Id);
                kids.Add(kid);
            }

            sponsor.SponsoredKids = kids;
            _context.Sponsors.Add(sponsor);
            _context.SaveChanges();

            ViewBag.SponsorName = sponsor.Name;
            ViewBag.Kids = sponsor.SponsoredKids;

            return View("SubmissionResult");
        }

        private bool SendEmailToSponsor(Sponsor sponsor)
        {

            return true;
        }
        // GET: Sponsors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await _context.Sponsors.FindAsync(id);
            if (sponsor == null)
            {
                return NotFound();
            }
            return View(sponsor);
        }

        // POST: Sponsors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Mobile,Language,CommunicationPrefrence,SponsoredKids")] Sponsor sponsor)
        {
            if (id != sponsor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sponsor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SponsorExists(sponsor.Id))
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
            return View(sponsor);
        }

        // GET: Sponsors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await _context.Sponsors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sponsor == null)
            {
                return NotFound();
            }

            return View(sponsor);
        }

        // POST: Sponsors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sponsor = await _context.Sponsors.FindAsync(id);
            _context.Sponsors.Remove(sponsor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SponsorExists(int id)
        {
            return _context.Sponsors.Any(e => e.Id == id);
        }

    }
}
