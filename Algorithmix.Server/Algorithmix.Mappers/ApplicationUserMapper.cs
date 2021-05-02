using Algorithmix.Entities;
using Algorithmix.Models;
using Algorithmix.Models.Account;

namespace Algorithmix.Mappers
{
    public class ApplicationUserMapper
    {
        public ApplicationUser ToModel(ApplicationUserEntity userEntity, string role)
        {
            if (userEntity == null)
                return null;

            return new ApplicationUser
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Role = role,
                Group = new Group { Id = userEntity.GroupId }
            };
        }

        public ApplicationUserEntity ToEntity(RegisterModel registerModel)
        {
            if (registerModel == null)
                return null;

            return new ApplicationUserEntity
            {
                Email = registerModel.Email,
                UserName = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                GroupId = registerModel.GroupId
            };
        }

        public AuthModel ToModel(ApplicationUserEntity userEntity, string role, string accessToken)
        {
            return new AuthModel
            {
                CurrentUser = ToModel(userEntity, role),
                AccessToken = accessToken
            };
        }
    }
}
