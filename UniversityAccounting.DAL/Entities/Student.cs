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

        public override bool Equals(object obj)
        {
            if (obj is not Student student) return false;

            return Id == student.Id && FirstName == student.FirstName && LastName == student.LastName
                   && GroupId == student.GroupId && DateOfBirth == student.DateOfBirth;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FirstName, LastName, GroupId, DateOfBirth);
        }
    }
}
