using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityAccounting.DAL.Entities
{
    public class Student : EntityBase
    {
        [Required]
        public int GroupId { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; }

        [Required]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Range(1, 4)]
        public int Status { get; set; } = 1;

        [Range(2.0, 5.0)]
        public double? FinalExamGpa { get; set; }

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
