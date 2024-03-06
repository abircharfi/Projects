using System;
using System.ComponentModel.DataAnnotations;

namespace IsportProject.Attributes;

public class ValidationDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return new ValidationResult("Birthdate is required.");
        }

        
        if (DateTime.TryParse(value.ToString(), out DateTime dateToCheck))
        {
            DateTime YearsAgo = DateTime.Now.AddYears(-15);

            if (dateToCheck > DateTime.Now)
            {
                return new ValidationResult("The date should be in the past!");
            }
            else if (dateToCheck >= YearsAgo)
            {
                return new ValidationResult("The date should be more than 15 years ago!");
            }

           
            return ValidationResult.Success;
        }
        else
        {
           
            return new ValidationResult("Invalid date format.");
        }
    }
}
