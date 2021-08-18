using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace StudentAccounting.Models
{
    public class Group
    {
        [Key]
        [Remote(action: "VerifyGroupName", controller: "Groups", AdditionalFields = nameof(Name))]
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Group name length can't be more than 30 characters.")]
        [Remote(action: "VerifyGroupName", controller: "Groups", AdditionalFields = nameof(Id))]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DisplayName("Formation Date")]
        public DateTime FormationDate { get; set; }

        public virtual ICollection<Student> Students { get; set; }
    }
}
