using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; } //TODO: Display attr Name= ??

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public int Status { get; set; } = 1;

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
