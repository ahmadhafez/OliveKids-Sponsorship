using OliveKids.Models;
using System;
//using System.Web.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OliveKids.Logic
{
    public class SponsorModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)//ModelBindingExecutionContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));


            if (bindingContext.ValueProvider == null)
                return Task.CompletedTask;

            var result = GetModelFromContext(bindingContext.ValueProvider);
            bindingContext.Model = result;
            bindingContext.Result = ModelBindingResult.Success(result);
            
            return Task.CompletedTask;
        }

        private Sponsor GetModelFromContext(IValueProvider provider)
        {
            string NAME = nameof(Sponsor.Name);
            string EMAIL = nameof(Sponsor.Email);
            string MOBILE = nameof(Sponsor.Mobile);
            string LANGUAGE = nameof(Sponsor.Language);
            string COMMUNICATION_PREFRENCE = nameof(Sponsor.CommunicationPrefrence);
            string NOTES = nameof(Sponsor.Notes);
            string PAYMENT_METHOD = nameof(Sponsor.PaymentMethod);
            string RECIEPT = nameof(Sponsor.Receipt);
            string ADDRESS = nameof(Sponsor.Address);
            string SPONSORED_KIDS = nameof(Sponsor.SponsoredKids);

            var name = provider.GetValue(NAME).FirstValue;
            var email = provider.GetValue(EMAIL).FirstValue;
            var mobile = provider.GetValue(MOBILE).FirstValue;
            var language = provider.GetValue(LANGUAGE).FirstValue;
            var communicationPrefrence = provider.GetValue(COMMUNICATION_PREFRENCE).FirstValue;
            var notes = provider.GetValue(NOTES).FirstValue;
            var paymentMethod = provider.GetValue(PAYMENT_METHOD).FirstValue;
            var reciept = provider.GetValue(RECIEPT).FirstValue;
            var address = provider.GetValue(ADDRESS).FirstValue;

            var sponsoredKids = provider.GetValue(SPONSORED_KIDS).Values.ToString().Split(',');
            

            List<Kid> kids = new List<Kid>();
            foreach (var id in sponsoredKids)
            {
                bool isId = Guid.TryParse(id, out Guid guid);
                if (isId)
                {
                    var kid = new Kid() { Id = guid };
                    kids.Add(kid);
                }
            }

            Sponsor sponsor = new Sponsor()
            {
                Name = name,
                Email = email,
                Mobile = mobile,
                Language = language,
                CommunicationPrefrence = communicationPrefrence,
                Notes = notes,
                PaymentMethod = paymentMethod,
                Receipt = reciept,
                Address =address,
                SponsoredKids = kids
            };


            return sponsor;
        }
    }
}

