using System.Collections.Generic;

namespace UniversityAccounting.WEB.Models
{
    public class CoursesIndexViewModel
    {
        public IEnumerable<CourseViewModel> Courses { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
