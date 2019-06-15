using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OliveKids.Models;
using OliveKids.Repository;

namespace OliveKids.Controllers
{
    public class HomeController : Controller
    {
        private readonly OkSposershipContext _context;
        public HomeController(OkSposershipContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var totalKids = _context.Kids.Count();
            var availableKids =  _context.Kids.Count( p => p.Sponsor == null);
            var sposredKids = totalKids - availableKids;

            var summary = new Summary()
            {
                SponsoredKids = sposredKids,
                AvailableKids = availableKids,
                TotalKids = totalKids
            };
            return View(summary);
        }

    }
}
