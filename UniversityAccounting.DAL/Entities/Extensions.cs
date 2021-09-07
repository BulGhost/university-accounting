using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityAccounting.DAL.Entities
{
    public static class Extensions
    {
        public static IEnumerable<Course> FindCoursesWithSearchText(this IEnumerable<Course> courses, string searchText)
        {
            if (string.IsNullOrEmpty(searchText)) return courses;

            searchText = searchText.ToLower();
            return courses.Where(c => c.Name.ToLower().Contains(searchText)
                                      || c.Description.ToLower().Contains(searchText));
        }

        public static IEnumerable<Group> FindGroupsWithSearchText(this IEnumerable<Group> groups, string searchText)
        {
            if (string.IsNullOrEmpty(searchText)) return groups;

            searchText = searchText.ToLower();
            return groups.Where(g => g.Name.ToLower().Contains(searchText) ||
                                     (int.TryParse(searchText, out int n) && g.StudentsQuantity == n) ||
                                     g.FormationDate.ToShortDateString().Contains(searchText));
        }

        public static IEnumerable<Student> FindStudentsWithSearchText(this IEnumerable<Student> students, string searchText)
        {
            if (string.IsNullOrEmpty(searchText)) return students;

            searchText = searchText.ToLower();
            return students.Where(s => s.FirstName.ToLower().Contains(searchText) ||
                                       s.LastName.ToLower().Contains(searchText) ||
                                       s.DateOfBirth.ToShortDateString().Contains(searchText));
        }
    }
}
