﻿using Algorithmix.Entities;
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
        private readonly ApplicationUserMapper _userMapper;
        private readonly UserManager<ApplicationUserEntity> _userManager;

        public UserService(ApplicationUserMapper userMapper, UserManager<ApplicationUserEntity> userManager)
        {
            _userMapper = userMapper;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            var userEntity = await _userManager.FindByIdAsync(id);

            if (userEntity == null)
                return null;

            var userRole = await _userManager.GetRoleAsync(userEntity);
            return _userMapper.ToModel(userEntity, userRole);
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var userEntity = await _userManager.FindByEmailAsync(email);

            if (userEntity == null)
                return null;

            var userRole = await _userManager.GetRoleAsync(userEntity);
            return _userMapper.ToModel(userEntity, userRole);
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
