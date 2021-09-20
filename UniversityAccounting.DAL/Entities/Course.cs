using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UniversityAccounting.DAL.Entities
{
    public class Course : EntityBase
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
    }
}
