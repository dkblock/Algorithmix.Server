using Algorithmix.Entities;
using Algorithmix.Models.Account;
using Algorithmix.Models.Groups;
using Algorithmix.Models.Users;
using System.Collections.Generic;
using System.Linq;

namespace Algorithmix.Mappers
{
    public class ApplicationUserMapper
    {
        public ApplicationUserEntity ToEntity(RegisterPayload registerPayload)
        {
            if (registerPayload == null)
                return null;

            return new ApplicationUserEntity
            {
                Email = registerPayload.Email,
                UserName = registerPayload.Email,
                FirstName = registerPayload.FirstName,
                LastName = registerPayload.LastName,
                GroupId = registerPayload.GroupId
            };
        }

        public ApplicationUser ToModel(ApplicationUserEntity userEntity)
        {
            if (userEntity == null)
                return null;

            return new ApplicationUser
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                EmailConfirmed = userEntity.EmailConfirmed,
                Group = new Group { Id = userEntity.GroupId }
            };
        }

        public UserAccount ToModel(ApplicationUser user, string accessToken)
        {
            return new UserAccount
            {
                CurrentUser = user,
                AccessToken = accessToken
            };
        }

        public ApplicationUserEntity UpdateEntity(ApplicationUserEntity userEntity, ApplicationUserPayload userPayload)
        {
            userEntity.EmailConfirmed = userEntity.EmailConfirmed && userEntity.Email == userPayload.Email;
            userEntity.Email = userPayload.Email ?? userEntity.Email;
            userEntity.FirstName = userPayload.FirstName ?? userEntity.FirstName;
            userEntity.LastName = userPayload.LastName ?? userEntity.LastName;
            userEntity.GroupId = userPayload.GroupId ?? userEntity.GroupId;

            return userEntity;
        }
    }
}
