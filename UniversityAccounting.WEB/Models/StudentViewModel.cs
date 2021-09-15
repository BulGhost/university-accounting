using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace UniversityAccounting.WEB.Models
{
    public class StudentViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Group")]
        public int GroupId { get; set; }

        [Display(Name = "Group")]
        public string GroupName { get; set; }

        [Required(ErrorMessage = "FirstNameRequired")]
        [StringLength(30, ErrorMessage = "FirstNameErrorMessage")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastNameRequired")]
        [StringLength(30, ErrorMessage = "LastNameErrorMessage")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "DateOfBirthRequired")]
        [DataType(DataType.Date)]
        [Display(Name = "DateOfBirth")]
        [Remote(action: "VerifyStudent", controller: "Students",
            AdditionalFields = nameof(FirstName) + "," + nameof(LastName) + "," + nameof(Id))]
        public DateTime DateOfBirth { get; set; } = new(2003, 1, 1);

        [Required(ErrorMessage = "StatusRequired")]
        [Range(1, 4, ErrorMessage = "ChooseStatus")]
        [Display(Name = "Status")]
        public int Status { get; set; }

        [DisplayName("GPA")]
        [Range(2.0, 5.0, ErrorMessage = "GpaRangeError")]
        [UIHint("Decimal")]
        [Display(Name = "Gpa")]
        public double? FinalExamGpa { get; set; }
    }
}
