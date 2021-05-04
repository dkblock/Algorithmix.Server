using Algorithmix.Common.Extensions;
using Algorithmix.Common.Validation;
using Algorithmix.Models.Groups;
using Algorithmix.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Algorithmix.Api.Validation
{
    public class GroupValidator
    {
        private readonly GroupService _groupService;

        public GroupValidator(GroupService groupService)
        {
            _groupService = groupService;
        }

        public async Task<ValidationResult> Validate(GroupPayload group, int? groupId = null)
        {
            var validationErrors = new List<ValidationError>();
            var groups = await _groupService.GetAllGroups();

            if (string.IsNullOrEmpty(group.Name))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(group.Name).ToCamelCase(),
                    Message = "Введите название"
                });

            if (groups.Any(g => g.Name == group.Name && g.Id != groupId))
                validationErrors.Add(new ValidationError
                {
                    Field = nameof(group.Name).ToCamelCase(),
                    Message = "Группа с таким названием уже существует"
                });

            return new ValidationResult
            {
                IsValid = !validationErrors.Any(),
                ValidationErrors = validationErrors
            };
        }
    }
}
