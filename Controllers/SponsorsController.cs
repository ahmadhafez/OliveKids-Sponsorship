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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace OliveKids.Controllers
{
    public class SponsorsController : Controller
    {
        private readonly OkSposershipContext _context;
        private readonly IHostingEnvironment _env = null;
        private readonly IConfiguration _configration;

        public SponsorsController(OkSposershipContext context, IHostingEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _env = env;
            _configration = configuration;
        }

        // GET: Sponsors
        public async Task<IActionResult> Index()
        {
            var result = await _context.Sponsors.Include
                (sponsor => sponsor.SponsoredKids).ToListAsync();
            return View(result);
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
        public IActionResult Create([ModelBinder(binderType: typeof(SponsorModelBinder))] Sponsor sponsor)
        {

            List<Kid> kids = new List<Kid>();
            foreach (var vKid in sponsor.SponsoredKids)
            {
                var kid = _context.Kids.Find(vKid.Id);
                kids.Add(kid);
            }

            sponsor.SponsoredKids = kids;
            _context.Sponsors.Add(sponsor);
            _context.SaveChanges();

            bool isEmailSent = SendEmailToSponsor(sponsor);

            if (isEmailSent)
            {
                sponsor.LastConfirmationEmail = DateTime.Now;
                _context.Sponsors.Update(sponsor);
                _context.SaveChangesAsync();
            }

            ViewBag.SponsorName = sponsor.Name;
            ViewBag.Kids = sponsor.SponsoredKids;
            ViewBag.IsEmailSent = isEmailSent;

            return View("SubmissionResult");

        }

        private bool SendEmailToSponsor(Sponsor sponsor)
        {
            try
            {
                var emailConfigrations = _configration.GetSection("Email");
                var senderName = emailConfigrations.GetSection("SenderName")?.Value;
                var senderEmail = emailConfigrations.GetSection("SenderEmail")?.Value;
                var userName = emailConfigrations.GetSection("Username")?.Value;
                var password = emailConfigrations.GetSection("Password")?.Value;
                var host = emailConfigrations.GetSection("Host")?.Value;
                var port = emailConfigrations.GetSection("Port")?.Value;
                var subject = emailConfigrations.GetSection("Subject")?.Value;
                var htmlBody = emailConfigrations.GetSection("HtmlBody")?.Value;
               // var textBody = emailConfigrations.GetSection("TextBody")?.Value;
                var inReplyTo = emailConfigrations.GetSection("InReplyTo")?.Value;

                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress(senderName, senderEmail);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(sponsor.Name, sponsor.Email);
                message.To.Add(to);

                message.Subject = string.Format(subject,sponsor.Name);

                
                BodyBuilder bodyBuilder = new BodyBuilder();
                StringBuilder kidHhtmlInfo = new StringBuilder();
                string rootPath = _env.WebRootPath;
                kidHhtmlInfo.Append("<table border ='1' style=' font-family: Arial; font-size: 11px;'><tr><th style='padding: 10px'>Name</th><th style='padding: 10px'>Arabic Name</th><th style='padding: 10px'>Age</th><th style='padding: 10px'> Gender </th></tr>");
                foreach(Kid kid in sponsor.SponsoredKids)
                {
                    kidHhtmlInfo.Append("<tr>");
                    var data = string.Format("<td style='padding: 10px'>{0}</td><td style='padding: 10px'>{1}</td><td style='padding: 10px'>{2}</td><td>{3}</td>", kid.Name, kid.ArabicName, kid.Age, kid.Gender);
                    kidHhtmlInfo.Append(data);
                    kidHhtmlInfo.Append("</tr>");
                    
                    string fileName = string.Format(@"{0}\kids\{1}.jpg",rootPath, kid.Id);
                    bodyBuilder.Attachments.Add(fileName);
                }

                kidHhtmlInfo.Append("</table>");
               

                htmlBody = string.Format(htmlBody, sponsor.Name, kidHhtmlInfo.ToString(),
                    sponsor.Name, sponsor.Mobile, sponsor.CommunicationPrefrence, sponsor.Language);
                bodyBuilder.HtmlBody = htmlBody;
                
                message.Body = bodyBuilder.ToMessageBody();
                message.InReplyTo = inReplyTo;


                using (SmtpClient client = new SmtpClient())
                {
                    client.Connect(host, Convert.ToInt32(port), true);
                    client.Authenticate(userName, password);
                    client.Send(message);
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("error", e.Message);
                return false;
            }
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
            if (sponsor.SponsoredKids != null)
            {
                foreach (var kid in sponsor.SponsoredKids)
                {
                    //kid.Sponsor = null;
                    //_context.Kids.Update(kid);
                }
            }
            
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
