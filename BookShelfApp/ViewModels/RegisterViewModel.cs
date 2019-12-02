using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShelfApp.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        public string Address { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
