using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAPI.Models
{
    public class ContactForm
    {
        [Required]
        [MinLength(4)]
        public string LastName { get; set; }
        
        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Telephone { get; set; }
        public bool IsFavorite { get; set; }
    }
}
