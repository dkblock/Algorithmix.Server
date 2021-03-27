using VisualAlgorithms.Domain;
using VisualAlgorithms.Entities;
using VisualAlgorithms.Models.Account;

namespace VisualAlgorithms.Mappers
{
    public class ApplicationUserMapper
    {
        public ApplicationUser ToDomain(ApplicationUserEntity userEntity, string role)
        {
            if (userEntity == null)
                return null;

            return new ApplicationUser
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                GroupId = userEntity.GroupId,
                Role = role
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
                CurrentUser = ToDomain(userEntity, role),
                AccessToken = accessToken
            };
        }
    }
}
