using Algorithmix.Models.Tests;
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
