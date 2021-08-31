using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace UniversityAccounting.WEB.Models
{
    public enum StudentStatus
    {
        Applicant = 1,
        Undergraduate,
        Graduate,
        Expelled
    }

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

        [Required]
        [StringLength(30)]
        [Display(Name = "FirstName", ResourceType = typeof(Resources.Models.StudentViewModel))]
        [Remote(action: "VerifyStudent", controller: "Students",
            AdditionalFields = nameof(LastName) + "," + nameof(DateOfBirth) + "," + nameof(Id))]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "LastName", ResourceType = typeof(Resources.Models.StudentViewModel))]
        [Remote(action: "VerifyStudent", controller: "Students",
            AdditionalFields = nameof(FirstName) + "," + nameof(DateOfBirth) + "," + nameof(Id))]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [Display(Name = "DateOfBirth", ResourceType = typeof(Resources.Models.StudentViewModel))]
        [Remote(action: "VerifyStudent", controller: "Students",
            AdditionalFields = nameof(FirstName) + "," + nameof(LastName) + "," + nameof(Id))]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Range(1, 4)]
        [Display(Name = "Status", ResourceType = typeof(Resources.Models.StudentViewModel))]
        public int Status { get; set; } = 1;

        [DisplayName("GPA")]
        [Range(2.0, 5.0)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}")]
        [Display(Name = "Gpa", ResourceType = typeof(Resources.Models.StudentViewModel))]
        public double FinalExamGpa { get; set; }
    }
}
