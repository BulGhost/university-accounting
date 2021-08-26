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

        public virtual ICollection<Student> Students { get; set; }
    }
}
