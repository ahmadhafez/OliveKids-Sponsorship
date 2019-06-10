using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OliveKids.Models
{
    [Table("Kid")]
    public class Kid
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Arabic name")]
        public string ArabicName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [MaxLength(2000)]
        [MinLength(50)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        
        [NotMapped]
        public object Photo { get; set; }
        [NotMapped]
        public int Age
        {
            get
            {
                // Save today's date.
                var today = DateTime.Today;
                // Calculate the age.
                var age = today.Year - DateOfBirth.Year;
                // Go back to the year the person was born in case of a leap year
                if (DateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }

}
