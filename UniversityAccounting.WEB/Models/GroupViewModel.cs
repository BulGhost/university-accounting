using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace UniversityAccounting.WEB.Models
{
    public class GroupViewModel
    {
        [Key]
        [Remote(action: "VerifyGroupName", controller: "Groups", AdditionalFields = nameof(Name))]
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required(ErrorMessageResourceName = "GroupNameRequired",
            ErrorMessageResourceType = typeof(Resources.Models.GroupViewModel))]
        [StringLength(30, ErrorMessageResourceName = "GroupNameErrorMessage",
            ErrorMessageResourceType = typeof(Resources.Models.GroupViewModel))]
        [Remote(action: "VerifyGroupName", controller: "Groups", AdditionalFields = nameof(Id))]
        [Display(Name = "Name", ResourceType = typeof(Resources.Models.GroupViewModel))]
        public string Name { get; set; }

        [Required(ErrorMessageResourceName = "GroupFormationDateRequired",
            ErrorMessageResourceType = typeof(Resources.Models.GroupViewModel))]
        [Column(TypeName = "date")]
        [Display(Name = "FormationDate", ResourceType = typeof(Resources.Models.GroupViewModel))]
        public DateTime FormationDate { get; set; }

        [Display(Name = "StudentsQuantity", ResourceType = typeof(Resources.Models.GroupViewModel))]
        public int StudentsQuantity { get; set; }
    }
}
