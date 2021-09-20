using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UniversityAccounting.DAL.Entities;

namespace UniversityAccounting.DAL.Interfaces
{
    public interface IGroupRepository : IRepository<Group>
    {
        int SuitableGroupsCount(Expression<Func<Group, bool>> predicate, string searchText);

        IEnumerable<Group> GetRequiredGroups(Expression<Func<Group, bool>> predicate, string searchText,
            string sortProperty, int pageIndex, int pageSize, SortOrder sortOrder = SortOrder.Ascending);
    }
}
