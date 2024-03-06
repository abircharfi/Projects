using System;
using System.ComponentModel.DataAnnotations;

namespace IsportProject.Attributes ;

public class EventDateValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
    
         if (((DateTime)value) <= DateTime.Now)
            {
                return new ValidationResult("Only dates in the future are allowed!");
            }
            return ValidationResult.Success;
    
    }
}
