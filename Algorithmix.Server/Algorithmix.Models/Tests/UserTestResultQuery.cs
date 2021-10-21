using System;
using System.Collections.Generic;

namespace Algorithmix.Models.Tests
{
    public class UserTestResultQuery
    {
        public UserTestResultQuery(string searchText, int groupId, UserTestResultSortBy sortBy, bool sortByDesc)
        {
            SearchText = searchText;
            GroupId = groupId;
            SortBy = sortBy;
            SortByDesc = sortByDesc;
        }

        public string SearchText { get; set; }
        public int GroupId { get; set; }
        public UserTestResultSortBy SortBy { get; set; }
        public bool SortByDesc { get; set; }

        
    }

    public enum UserTestResultSortBy
    {
        FullName,
        GroupName,
        TestName,
        Result,
        PassingTime,
    }
}
