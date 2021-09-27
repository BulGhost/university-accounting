using System.Collections.Generic;
using UniversityAccounting.WEB.Controllers.HelperClasses;
using UniversityAccounting.WEB.Models.HelperClasses;

namespace UniversityAccounting.WEB.Models
{
    public class StudentsIndexViewModel
    {
        public IEnumerable<StudentViewModel> Students { get; set; }
        public ISortModel SortModel { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
