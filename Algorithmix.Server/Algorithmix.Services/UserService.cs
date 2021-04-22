using Algorithmix.Entities;
using Algorithmix.Identity;
using Algorithmix.Mappers;
using Algorithmix.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Algorithmix.Services
{
    public class UserService
    {
        private readonly AuthenticationService _authService;
        private readonly ApplicationUserMapper _userMapper;
        private readonly UserManager<ApplicationUserEntity> _userManager;

        public UserService(AuthenticationService authService, ApplicationUserMapper userMapper, UserManager<ApplicationUserEntity> userManager)
        {
            _authService = authService;
            _userMapper = userMapper;
            _userManager = userManager;
        }

        public string GetUserIdByAccessToken(string authorization)
        {
            var accessToken = authorization.Replace("Bearer ", "");
            var authModel = _authService.CheckAuth(accessToken);

            return authModel.CurrentUser.Id;
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            var userEntity = await _userManager.FindByIdAsync(id);
            var userRole = await _userManager.GetRoleAsync(userEntity);

            return _userMapper.ToDomain(userEntity, userRole);
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var userEntity = await _userManager.FindByEmailAsync(email);
            var userRole = await _userManager.GetRoleAsync(userEntity);

            return _userMapper.ToDomain(userEntity, userRole);
        }

        public async Task<bool> IsPasswordValid(string email, string password)
        {
            var userEntity = await _userManager.FindByEmailAsync(email);

            if (userEntity != null)
                return await _userManager.CheckPasswordAsync(userEntity, password);

            return false;
        }

        public async Task<ApplicationUserEntity> GetUser(ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }
    }
}
