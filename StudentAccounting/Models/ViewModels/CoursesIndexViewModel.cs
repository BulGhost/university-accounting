using System.Collections.Generic;

namespace StudentAccounting.Models.ViewModels
{
    public class CoursesIndexViewModel
    {
        public IEnumerable<Course> Courses { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
