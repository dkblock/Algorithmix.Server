using Algorithmix.Identity;
using Algorithmix.Mappers;
using Algorithmix.Models.Account;
using System.Threading.Tasks;

namespace Algorithmix.Api.Core
{
    public class AccountManager
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationUserMapper _userMapper;
        private readonly AuthenticationService _authService;

        public AccountManager(ApplicationUserManager userManager, ApplicationUserMapper userMapper, AuthenticationService authService)
        {
            _userManager = userManager;
            _userMapper = userMapper;
            _authService = authService;
        }

        public async Task<UserAccount> Authenticate(string authorization)
        {
            var authModel = _authService.CheckAuth(authorization);
            var user = await _userManager.GetUserById(authModel.CurrentUser.Id);

            return _userMapper.ToModel(user, authModel.AccessToken);
        }

        public async Task<UserAccount> Register(RegisterPayload registerPayload)
        {
            var user = await _userManager.CreateUser(registerPayload);
            var accessToken = _authService.Authenticate(user);

            return _userMapper.ToModel(user, accessToken);
        }

        public async Task<UserAccount> Login(LoginPayload loginPayload)
        {
            var user = await _userManager.GetUserByEmail(loginPayload.Email);
            var accessToken = _authService.Authenticate(user);

            return _userMapper.ToModel(user, accessToken);
        }
    }
}
