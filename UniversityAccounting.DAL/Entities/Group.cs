using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityAccounting.DAL.Entities
{
    public class Group : EntityBase
    {
        [Required]
        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime FormationDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int StudentsQuantity { get; private set; }

        public virtual ICollection<Student> Students { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not Group group) return false;

            return Id == group.Id && Name == group.Name && CourseId == group.CourseId &&
                   FormationDate == group.FormationDate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, CourseId, FormationDate);
        }
    }
}
