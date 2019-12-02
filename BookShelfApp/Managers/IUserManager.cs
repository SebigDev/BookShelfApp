using BookShelfApp.Models;
using BookShelfApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShelfApp.Managers
{
    public interface IUserManager
    {
        Task<User> RegisterUser(RegisterViewModel viewModel);
        Task<TokenModel> Login(LoginViewModel viewModel);

        Task<User> GetUser(long userId);
    }
}
