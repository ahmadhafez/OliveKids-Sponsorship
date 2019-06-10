using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using OliveKids.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace OliveKids.Models.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SponsorsApiController : Controller
    {
        private OkSposershipContext _context;

        public SponsorsApiController(OkSposershipContext context) {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions) {
            var sponsors = _context.Sponsors.Select(i => new {
                i.Id,
                i.Name,
                i.Email,
                i.Mobile,
                i.Language,
                i.CommunicationPrefrence
            });
            return Json(DataSourceLoader.Load(sponsors, loadOptions));
        }

        [HttpPost]
        public IActionResult Post(string values) {
            var model = new Sponsor();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Sponsors.Add(model);
            _context.SaveChanges();

            return Json(result.Entity.Id);
        }

        [HttpPut]
        public IActionResult Put(int key, string values) {
            var model = _context.Sponsors.FirstOrDefault(item => item.Id == key);
            if(model == null)
                return StatusCode(409, "Sponsor not found");

            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public void Delete(int key) {
            var model = _context.Sponsors.FirstOrDefault(item => item.Id == key);

            _context.Sponsors.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(Sponsor model, IDictionary values) {
            string ID = nameof(Sponsor.Id);
            string NAME = nameof(Sponsor.Name);
            string EMAIL = nameof(Sponsor.Email);
            string MOBILE = nameof(Sponsor.Mobile);
            string LANGUAGE = nameof(Sponsor.Language);
            string COMMUNICATION_PREFRENCE = nameof(Sponsor.CommunicationPrefrence);

            if(values.Contains(ID)) {
                model.Id = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(NAME)) {
                model.Name = Convert.ToString(values[NAME]);
            }

            if(values.Contains(EMAIL)) {
                model.Email = Convert.ToString(values[EMAIL]);
            }

            if(values.Contains(MOBILE)) {
                model.Mobile = Convert.ToString(values[MOBILE]);
            }

            if(values.Contains(LANGUAGE)) {
                model.Language = Convert.ToString(values[LANGUAGE]);
            }

            if(values.Contains(COMMUNICATION_PREFRENCE)) {
                model.CommunicationPrefrence = Convert.ToString(values[COMMUNICATION_PREFRENCE]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}