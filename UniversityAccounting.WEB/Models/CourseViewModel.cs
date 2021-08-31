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
        [StringLength(30, ErrorMessageResourceName = "CourseNameErrorMessage",
            ErrorMessageResourceType = typeof(Resources.Models.CourseViewModel))]
        [Remote(action: "VerifyCourseName", controller: "Courses", AdditionalFields = nameof(Id))]
        [Display(Name = "Name", ResourceType = typeof(Resources.Models.CourseViewModel))]
        public string Name { get; set; }

        [StringLength(300, ErrorMessageResourceName = "DescriptionErrorMessage",
            ErrorMessageResourceType = typeof(Resources.Models.CourseViewModel))]
        [Display(Name = "Description", ResourceType = typeof(Resources.Models.CourseViewModel))]
        public string Description { get; set; }
    }
}
