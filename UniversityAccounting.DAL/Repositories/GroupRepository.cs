using UniversityAccounting.DAL.EF;
using UniversityAccounting.DAL.Interfaces;

namespace UniversityAccounting.DAL.Repositories
{
    public class GroupRepository : BaseRepository<Entities.Group>, IGroupRepository
    {
        public UniversityContext UniversityContext => Context as UniversityContext;

        public GroupRepository(UniversityContext context) : base(context)
        {
        }
    }
}
