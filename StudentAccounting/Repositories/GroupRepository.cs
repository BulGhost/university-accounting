using System.Linq;
using StudentAccounting.Data;
using StudentAccounting.Repositories.Interfaces;

namespace StudentAccounting.Repositories
{
    public class GroupRepository : BaseRepository<Models.Group>, IGroupRepository
    {
        public UniversityContext UniversityContext => Context as UniversityContext;

        public GroupRepository(UniversityContext context) : base(context)
        {
        }
    }
}
