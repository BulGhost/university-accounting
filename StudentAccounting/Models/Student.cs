using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace StudentAccounting.Models
{
    public enum StudentStatus
    {
        Applicant = 1,
        Undergraduate,
        Graduate,
        Expelled
    }

    public class Student
    {
        [Key]
        [Remote(action: "VerifyStudent", controller: "Students",
            AdditionalFields = nameof(LastName) + "," + nameof(DateOfBirth) + "," + nameof(FirstName))]
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; }

        [Required]
        [StringLength(30)]
        [DisplayName("First Name")]
        [Remote(action: "VerifyStudent", controller: "Students",
            AdditionalFields = nameof(LastName) + "," + nameof(DateOfBirth) + "," + nameof(Id))]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [DisplayName("Last Name")]
        [Remote(action: "VerifyStudent", controller: "Students",
            AdditionalFields = nameof(FirstName) + "," + nameof(DateOfBirth) + "," + nameof(Id))]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DisplayName("Date of Birth")]
        [Remote(action: "VerifyStudent", controller: "Students",
            AdditionalFields = nameof(FirstName) + "," + nameof(LastName) + "," + nameof(Id))]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Range(1, 4)]
        public int Status { get; set; } = 1;

        [DisplayName("GPA")]
        [Range(2.0, 5.0)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F2}")]
        public double FinalExamGpa { get; set; }

        public Student()
        {
        }

        public Student(int groupId, string firstName, string lastName)
        {
            GroupId = groupId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
