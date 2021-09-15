using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using UniversityAccounting.WEB.Models.HelperClasses;

namespace UniversityAccounting.WEB.Models
{
    public class GroupViewModel
    {
        [Key]
        [Remote(action: "VerifyGroupName", controller: "Groups", AdditionalFields = nameof(Name))]
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "GroupNameRequired")]
        [StringLength(30, ErrorMessage = "GroupNameErrorMessage")]
        [Remote(action: "VerifyGroupName", controller: "Groups", AdditionalFields = nameof(Id))]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "GroupFormationDateRequired")]
        [DataType(DataType.Date)]
        [WithinSixYears]
        [Display(Name = "FormationDate")]
        public DateTime FormationDate { get; set; }

        [Display(Name = "StudentsQuantity")]
        public int StudentsQuantity { get; set; }
    }
}
