using System;
using System.ComponentModel.DataAnnotations;

 #pragma warning disable CS8765
 #pragma warning disable CS8603

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
