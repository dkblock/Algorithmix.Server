using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VisualAlgorithms.Common.Constants;
using VisualAlgorithms.Entities;
using VisualAlgorithms.Identity;
using VisualAlgorithms.Mappers;
using VisualAlgorithms.Models.Account;

namespace VisualAlgorithms.Services
{
    public class AccountService
    {
        private readonly AuthenticationService _authService;
        private readonly ApplicationUsersMapper _usersMapper;
        private readonly UserManager<ApplicationUserEntity> _userManager;

        public AccountService(
            AuthenticationService authService,
            ApplicationUsersMapper usersMapper,
            UserManager<ApplicationUserEntity> userManager)
        {
            _authService = authService;
            _usersMapper = usersMapper;
            _userManager = userManager;
        }

        public async Task<string> Register(RegisterModel registerModel)
        {
            var userEntity = _usersMapper.ToEntity(registerModel);
            var result = await _userManager.CreateAsync(userEntity, registerModel.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userEntity, Roles.User);
                return _authService.Authenticate(userEntity, Roles.User);
            }

            return null;
        }

        public async Task<string> Login(LoginModel loginModel)
        {
            var userEntity = await _userManager.FindByEmailAsync(loginModel.Email);
            var userRole = await _userManager.GetRoleAsync(userEntity);

            return _authService.Authenticate(userEntity, userRole);
        }
    }
}
