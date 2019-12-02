using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShelfApp.Models
{
    public class TokenModel
    {

        public long UserId { get; set; }
        public string Token { get; set; }
        public int Role { get; set; } = 1;
        public bool IsActive { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string WalletNumber { get; set; }
        public string AccountNumber { get; set; }
    }
}
