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

        public async Task<AuthModel> Register(RegisterModel registerModel)
        {
            var userEntity = _usersMapper.ToEntity(registerModel);
            var result = await _userManager.CreateAsync(userEntity, registerModel.Password);

            if (result.Succeeded)
            {
                var userRole = Roles.User;
                await _userManager.AddToRoleAsync(userEntity, userRole);
                var accessToken = _authService.Authenticate(userEntity, userRole);

                return _usersMapper.ToModel(userEntity, userRole, accessToken);
            }

            return null;
        }

        public async Task<AuthModel> Login(LoginModel loginModel)
        {
            var userEntity = await _userManager.FindByEmailAsync(loginModel.Email);
            var userRole = await _userManager.GetRoleAsync(userEntity);
            var accessToken = _authService.Authenticate(userEntity, userRole);

            return _usersMapper.ToModel(userEntity, userRole, accessToken);
        }
    }
}
