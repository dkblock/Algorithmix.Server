using VisualAlgorithms.Domain;
using VisualAlgorithms.Entities;
using VisualAlgorithms.Models.Account;

namespace VisualAlgorithms.Mappers
{
    public class ApplicationUsersMapper
    {
        public ApplicationUser ToDomain(ApplicationUserEntity userEntity)
        {
            if (userEntity == null)
                return null;

            return new ApplicationUser
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                GroupId = userEntity.GroupId
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
    }
}
