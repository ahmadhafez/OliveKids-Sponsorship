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
    public class KidsApiController : Controller
    {
        private OkSposershipContext _context;

        public KidsApiController(OkSposershipContext context) {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions) {
            var kids = _context.Kids.Select(i => new {
                i.Id,
                i.Name,
                i.DateOfBirth,
                i.Description
            });
            return Json(DataSourceLoader.Load(kids, loadOptions));
        }

        [HttpPost]
        public IActionResult Post(string values) {
            var model = new Kid();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Kids.Add(model);
            _context.SaveChanges();

            return Json(result.Entity.Id);
        }

        [HttpPut]
        public IActionResult Put(Guid key, string values) {
            var model = _context.Kids.FirstOrDefault(item => item.Id == key);
            if(model == null)
                return StatusCode(409, "Kid not found");

            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public void Delete(Guid key) {
            var model = _context.Kids.FirstOrDefault(item => item.Id == key);

            _context.Kids.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(Kid model, IDictionary values) {
            string ID = nameof(Kid.Id);
            string NAME = nameof(Kid.Name);
            string DATE_OF_BIRTH = nameof(Kid.DateOfBirth);
            string DESCRIPTION = nameof(Kid.Description);

            if(values.Contains(ID)) {
                model.Id = (System.Guid)Convert.ChangeType(values[ID], typeof(System.Guid));
            }

            if(values.Contains(NAME)) {
                model.Name = Convert.ToString(values[NAME]);
            }

            if(values.Contains(DATE_OF_BIRTH)) {
                model.DateOfBirth = Convert.ToDateTime(values[DATE_OF_BIRTH]);
            }

            if(values.Contains(DESCRIPTION)) {
                model.Description = Convert.ToString(values[DESCRIPTION]);
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