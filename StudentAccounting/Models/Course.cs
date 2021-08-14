using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentAccounting.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
    }
}
