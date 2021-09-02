﻿using System.Collections.Generic;
using UniversityAccounting.WEB.Models.HelperClasses;

namespace UniversityAccounting.WEB.Models
{
    public class CoursesIndexViewModel
    {
        public IEnumerable<CourseViewModel> Courses { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
