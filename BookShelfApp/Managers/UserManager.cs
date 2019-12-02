using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShelfApp.DbContextConfig;
using BookShelfApp.Helpers;
using BookShelfApp.Models;
using BookShelfApp.PasswordHelper;
using BookShelfApp.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BookShelfApp.Managers
{
    public class UserManager : IUserManager
    {
        private readonly BookShelfAppDbContext _bookShelfAppDbContext;
        private readonly IPasswordHasher _hasher;

        public UserManager(BookShelfAppDbContext bookShelfAppDbContext, IPasswordHasher hasher)
        {
            _bookShelfAppDbContext = bookShelfAppDbContext;
            _hasher = hasher; ;
        }
        public async Task<User> GetUser(long userId)
        {
            if(userId > 0)
            {
                var user = await _bookShelfAppDbContext.User.FirstOrDefaultAsync(u => u.Id == userId);
                if(user != null)
                {
                    return user;
                }
            }
            return null;
        }

        public async Task<TokenModel> Login(LoginViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            var user = await _bookShelfAppDbContext.User.FirstOrDefaultAsync(x => x.EmailAddress == viewModel.EmailAddress);

            if (user == null || user.IsActive == false) return null;
            //verify Password

            var isUser = _hasher.WegoPayDecrypt(user.Password, viewModel.Password);
            if (!isUser.Verified) return null;

            string token = UtilitySecurity.GenerateToken(user);
           
            var userToken = new TokenModel
            {
                UserId = user.Id,
                Token = token,
                IssuedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(3),
               
            };
            return userToken;

        }

        public async Task<User> RegisterUser(RegisterViewModel viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));
            var user = new User
            {
                Firstname = viewModel.Firstname,
                Lastname = viewModel.Lastname,
                EmailAddress = viewModel.EmailAddress,
                Password = _hasher.WegoPayEncrypt(viewModel.Password),
                RegisteredOn = DateTime.UtcNow,
                IsActive = true,
            };

            await _bookShelfAppDbContext.AddAsync(user);
            await _bookShelfAppDbContext.SaveChangesAsync();
            return user;
        }
    }
}
