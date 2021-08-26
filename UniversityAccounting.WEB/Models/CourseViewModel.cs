using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace UniversityAccounting.WEB.Models
{
    public class CourseViewModel
    {
        [Key]
        [Remote(action: "VerifyCourseName", controller: "Courses", AdditionalFields = nameof(Name))]
        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Course name length can't be more than 30 characters.")]
        [Remote(action: "VerifyCourseName", controller: "Courses", AdditionalFields = nameof(Id))]
        public string Name { get; set; }

        [StringLength(300, ErrorMessage = "Course description length can't be more than 300 characters.")]
        public string Description { get; set; }
    }
}
