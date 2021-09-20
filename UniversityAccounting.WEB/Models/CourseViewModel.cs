using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace UniversityAccounting.WEB.Models
{
    public class CourseViewModel
    {
        [Key]
        [Remote(action: "VerifyCourseName", controller: "Courses", AdditionalFields = nameof(Name))]
        public int Id { get; set; }

        [Required(ErrorMessage = "CourseNameRequired")]
        [StringLength(30, ErrorMessage = "CourseNameErrorMessage")]
        [Remote(action: "VerifyCourseName", controller: "Courses", AdditionalFields = nameof(Id))]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [StringLength(300, ErrorMessage = "DescriptionErrorMessage")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
