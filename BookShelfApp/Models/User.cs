using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShelfApp.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNo { get; set; }
        public string Password { get; set; }

        public bool IsActive { get; set; }
        public DateTime RegisteredOn { get; set; }
    }
}
