using Algorithmix.Common.Constants;
using Algorithmix.Entities;
using Algorithmix.Identity;
using Algorithmix.Mappers;
using Algorithmix.Models.Account;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Algorithmix.Services
{
    public class AccountService
    {
        private readonly AuthenticationService _authService;
        private readonly ApplicationUserMapper _userMapper;
        private readonly UserManager<ApplicationUserEntity> _userManager;

        public AccountService(
            AuthenticationService authService,
            ApplicationUserMapper userMapper,
            UserManager<ApplicationUserEntity> userManager)
        {
            _authService = authService;
            _userMapper = userMapper;
            _userManager = userManager;
        }

        public async Task<AuthModel> Authenticate(string authorization)
        {
            var authModel = _authService.CheckAuth(authorization);
            var userEntity = await _userManager.FindByIdAsync(authModel.CurrentUser.Id);

            return _userMapper.ToModel(userEntity, authModel.CurrentUser.Role, authModel.AccessToken);
        }

        public async Task<AuthModel> Login(LoginModel loginModel)
        {
            var userEntity = await _userManager.FindByEmailAsync(loginModel.Email);
            var userRole = await _userManager.GetRoleAsync(userEntity);
            var user = _userMapper.ToDomain(userEntity, userRole);
            var accessToken = _authService.Authenticate(user);

            return _userMapper.ToModel(userEntity, userRole, accessToken);
        }

        public async Task<AuthModel> Register(RegisterModel registerModel)
        {
            var userEntity = _userMapper.ToEntity(registerModel);
            var result = await _userManager.CreateAsync(userEntity, registerModel.Password);

            if (result.Succeeded)
            {
                var userRole = Roles.User;
                await _userManager.AddToRoleAsync(userEntity, userRole);
                var user = _userMapper.ToDomain(userEntity, userRole);
                var accessToken = _authService.Authenticate(user);

                return _userMapper.ToModel(userEntity, userRole, accessToken);
            }

            return null;
        }        
    }
}
