using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OliveKids.Models;
using OliveKids.Repository;

namespace OliveKids.Controllers
{
    public class HomeController : Controller
    {
        private readonly OkSposershipContext _context;
        IHostingEnvironment _env;
        public HomeController(OkSposershipContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {

            //foreach (Kid kid in _context.Kids)
            //{
            //    try
            //    {
            //        string directory = string.Format(@"{0}\kids\{1}", _env.WebRootPath, kid.Id);
            //        Directory.CreateDirectory(directory);
            //        string oldFile = string.Format(@"{0}\kids\{1}.jpg", _env.WebRootPath, kid.Id);
            //        string newFile = string.Format(@"{0}\{1}.jpg", directory, kid.Name);

            //        System.IO.File.Copy(oldFile, newFile);
            //    }catch
            //    {

            //    }
            //}

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
