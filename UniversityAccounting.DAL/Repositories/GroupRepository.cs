using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using UniversityAccounting.DAL.EF;
using UniversityAccounting.DAL.Entities;
using UniversityAccounting.DAL.Interfaces;

namespace UniversityAccounting.DAL.Repositories
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        public UniversityContext UniversityContext => Context as UniversityContext;

        public GroupRepository(UniversityContext context) : base(context)
        {
        }

        public int SuitableGroupsCount(Expression<Func<Group, bool>> predicate, string searchText)
        {
            return FilterGroups(predicate, searchText).Count();
        }


        public IEnumerable<Group> GetRequiredGroups(Expression<Func<Group, bool>> predicate, string searchText, string sortProperty, int pageIndex, int pageSize,
            SortOrder sortOrder = SortOrder.Ascending)
        {
            var filteredGroups = FilterGroups(predicate, searchText);
            var expr = GetKeySelector(typeof(Group), sortProperty);

            var requiredGroups = sortOrder == SortOrder.Ascending
                ? filteredGroups.OrderBy(expr)
                : filteredGroups.OrderByDescending(expr);

            return requiredGroups.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        private IQueryable<Group> FilterGroups(Expression<Func<Group, bool>> predicate, string searchText)
        {
            var groups = UniversityContext.Set<Group>().Where(predicate);
            if (string.IsNullOrEmpty(searchText)) return groups;

            searchText = searchText.ToLower();
            bool isNumber = int.TryParse(searchText, out int n);
            bool isDate = DateTime.TryParse(searchText, out var date);
            return groups.Where(g => g.Name.ToLower().Contains(searchText) ||
                                     isNumber && g.StudentsQuantity == n ||
                                     isDate && g.FormationDate == date);
        }
    }
}
