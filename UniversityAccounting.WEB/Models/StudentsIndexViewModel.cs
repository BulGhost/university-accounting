using System.Collections.Generic;

namespace UniversityAccounting.WEB.Models
{
    public class StudentsIndexViewModel
    {
        public IEnumerable<StudentViewModel> Students { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
