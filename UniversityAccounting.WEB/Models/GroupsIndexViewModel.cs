using System.Collections.Generic;

namespace UniversityAccounting.WEB.Models
{
    public class GroupsIndexViewModel
    {
        public IEnumerable<GroupViewModel> Groups { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
