using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Components.Forms;

namespace WebApi.Validators;

public class DateTimeAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var date= (DateTime?)value;
        if(date<DateTime.Now){
        return new   ValidationResult("This date must be greater than today's date");
        }
        return ValidationResult.Success;
    }
    

}
