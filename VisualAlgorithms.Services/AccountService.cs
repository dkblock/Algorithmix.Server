using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VisualAlgorithms.Entities;

namespace VisualAlgorithms.Services
{
    public class AccountService
    {
        private readonly UserManager<ApplicationUserEntity> _userManager;

        public AccountService(UserManager<ApplicationUserEntity> userManager)
        {
            _userManager = userManager;
        }
    }
}
