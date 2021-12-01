using Algorithmix.Models.Algorithms;
using Algorithmix.Models.Groups;
using Algorithmix.Models.Tests;
using Algorithmix.Models.Users;
using System;
using System.Collections.Generic;

namespace Algorithmix.Api.Core.Helpers
{
    public class QueryHelper
    {
        public bool IsMatch(string searchText, string[] filters)
        {
            if (string.IsNullOrEmpty(searchText))
                return true;

            foreach (var filter in filters)
            {
                if (filter.ToLower().Contains(searchText.ToLower()))
                    return true;
            }

            return false;
        }

        public IDictionary<AlgorithmSortBy, Func<Algorithm, object>> AlgorithmSortModel =>
            new Dictionary<AlgorithmSortBy, Func<Algorithm, object>>
            {
                { AlgorithmSortBy.None, x => x.TimeComplexity.Id },
                { AlgorithmSortBy.Id, x => x.Id },
                { AlgorithmSortBy.Name, x => x.Name },
                { AlgorithmSortBy.CreatedBy, x => $"{x.CreatedBy.LastName} {x.CreatedBy.FirstName}" },
                { AlgorithmSortBy.HasDescription, x => x.HasDescription },
                { AlgorithmSortBy.HasConstructor, x => x.HasConstructor },
            };

        public IDictionary<ApplicationUserSortBy, Func<ApplicationUser, object>> ApplicationUserSortModel =>
            new Dictionary<ApplicationUserSortBy, Func<ApplicationUser, object>>
            {
                { ApplicationUserSortBy.FullName, x =>  $"{x.LastName} {x.FirstName}" },
                { ApplicationUserSortBy.Email, x =>  x.Email },
                { ApplicationUserSortBy.GroupId, x =>  x.Group.Id },
                { ApplicationUserSortBy.Role, x =>  x.Role },
            };

        public IDictionary<GroupSortBy, Func<Group, object>> GroupSortModel =>
            new Dictionary<GroupSortBy, Func<Group, object>>
            {
                { GroupSortBy.Id, x => x.Id },
                { GroupSortBy.Name, x => x.Name },
                { GroupSortBy.UsersCount, x => x.UsersCount },
                { GroupSortBy.IsAvailableForRegister, x => x.IsAvailableForRegister },
            };

        public IDictionary<TestSortBy, Func<Test, object>> TestSortModel =>
            new Dictionary<TestSortBy, Func<Test, object>>
            {
                { TestSortBy.Name, x => x.Name },
                { TestSortBy.Status, x => x.UserResult?.IsPassed },
                { TestSortBy.IsPublished, x => x.IsPublished },
                { TestSortBy.AverageResult, x => x.AverageResult },
                { TestSortBy.CreatedBy, x => $"{x.CreatedBy.LastName} {x.CreatedBy.FirstName}" },
                { TestSortBy.CreatedDate, x => x.CreatedDate },
                { TestSortBy.UpdatedDate, x => x.UpdatedDate },
            };

        public IDictionary<UserTestResultSortBy, Func<UserTestResult, object>> UserTestResultSortModel =>
            new Dictionary<UserTestResultSortBy, Func<UserTestResult, object>>
            {
                { UserTestResultSortBy.FullName, x => $"{x.User.LastName} {x.User.FirstName}" },
                { UserTestResultSortBy.GroupName, x => x.User.Group.Id },
                { UserTestResultSortBy.TestName, x => x.Test.Name },
                { UserTestResultSortBy.Result, x => x.Result },
                { UserTestResultSortBy.PassingTime, x => x.PassingTime },
            };
    }
}
