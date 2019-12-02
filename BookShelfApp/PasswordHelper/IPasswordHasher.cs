using System;
using System.Collections.Generic;
using System.Text;

namespace BookShelfApp.PasswordHelper
{
    public interface IPasswordHasher
    {
        string WegoPayEncrypt(string password);

        (bool Verified, bool NeedsUpgrade) WegoPayDecrypt(string hash, string password);

    }
}
