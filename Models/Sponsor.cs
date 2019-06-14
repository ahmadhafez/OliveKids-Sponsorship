using Microsoft.AspNetCore.Mvc;
using OliveKids.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OliveKids.Models
{
    [Table("Sponsor")]
    [ModelBinder(BinderType = typeof(SponsorModelBinder))]
    public class Sponsor
    {
        //public Sponsor()
        //{
        //    this.SponsoredKids = new List<Kid>();
        //}
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required]
        public string Mobile { get; set; }
        [DisplayName("Reports Language")]
        public string Language { get; set; }
        [DisplayName("Communication Preference")]
        public string CommunicationPrefrence { get; set; }
        [MaxLength(4000)]
        public string Notes { get; set; }
        [DisplayName("Payment method")]
        public string PaymentMethod { get; set; }
        public string Receipt { get; set; }
        public string Address { get; set; }

        [Required]
        public List<Kid> SponsoredKids { get; set; }
       
    }
}
