using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace UniversityAccounting.WEB.Models
{
    public class StudentViewModel
    {
        [Key]
        [Remote(action: "VerifyStudent", controller: "Students",
            AdditionalFields = nameof(LastName) + "," + nameof(DateOfBirth) + "," + nameof(FirstName))]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Group", ResourceType = typeof(Resources.Models.StudentViewModel))]
        public int GroupId { get; set; }

        [Display(Name = "Group", ResourceType = typeof(Resources.Models.StudentViewModel))]
        public string GroupName { get; set; }

        [Required(ErrorMessageResourceName = "FirstNameRequired",
            ErrorMessageResourceType = typeof(Resources.Models.StudentViewModel))]
        [StringLength(30, ErrorMessageResourceName = "FirstNameErrorMessage",
            ErrorMessageResourceType = typeof(Resources.Models.StudentViewModel))]
        [Display(Name = "FirstName", ResourceType = typeof(Resources.Models.StudentViewModel))]
        [Remote(action: "VerifyStudent", controller: "Students",
            AdditionalFields = nameof(LastName) + "," + nameof(DateOfBirth) + "," + nameof(Id))]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceName = "LastNameRequired",
            ErrorMessageResourceType = typeof(Resources.Models.StudentViewModel))]
        [StringLength(30, ErrorMessageResourceName = "LastNameErrorMessage",
            ErrorMessageResourceType = typeof(Resources.Models.StudentViewModel))]
        [Display(Name = "LastName", ResourceType = typeof(Resources.Models.StudentViewModel))]
        [Remote(action: "VerifyStudent", controller: "Students",
            AdditionalFields = nameof(FirstName) + "," + nameof(DateOfBirth) + "," + nameof(Id))]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceName = "DateOfBirthRequired",
            ErrorMessageResourceType = typeof(Resources.Models.StudentViewModel))]
        [Column(TypeName = "date")]
        [Display(Name = "DateOfBirth", ResourceType = typeof(Resources.Models.StudentViewModel))]
        [Remote(action: "VerifyStudent", controller: "Students",
            AdditionalFields = nameof(FirstName) + "," + nameof(LastName) + "," + nameof(Id))]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessageResourceName = "StatusRequired",
            ErrorMessageResourceType = typeof(Resources.Models.StudentViewModel))]
        [Range(1, 4, ErrorMessageResourceName = "ChooseStatus",
            ErrorMessageResourceType = typeof(Resources.Models.StudentViewModel))]
        [Display(Name = "Status", ResourceType = typeof(Resources.Models.StudentViewModel))]
        public int Status { get; set; }

        [DisplayName("GPA")]
        [Range(2.0, 5.0, ErrorMessageResourceName = "GpaRangeError",
            ErrorMessageResourceType = typeof(Resources.Models.StudentViewModel))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}")]
        [Display(Name = "Gpa", ResourceType = typeof(Resources.Models.StudentViewModel))]
        public double? FinalExamGpa { get; set; }
    }
}
