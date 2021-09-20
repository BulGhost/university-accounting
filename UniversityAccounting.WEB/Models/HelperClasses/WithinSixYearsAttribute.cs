using System;
using System.ComponentModel.DataAnnotations;

namespace UniversityAccounting.WEB.Models.HelperClasses
{
    public class WithinSixYearsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            value = (DateTime) value;
            if (DateTime.Now.AddYears(-6).CompareTo(value) <= 0 && DateTime.Now.CompareTo(value) >= 0)
                return ValidationResult.Success;

            return new ValidationResult(Resources.Models.GroupViewModel.FormationDateError);
        }
    }
}
