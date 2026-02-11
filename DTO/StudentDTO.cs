using System;
using System.ComponentModel.DataAnnotations;
using WebApi.Validators;

namespace WebApi.Controllers.DTO;

public class StudentDTO
{
      public int id{get; set;}
      [Required (ErrorMessage ="Enter Student name")]

      [StringLength(70)]
    public string StudentName{get; set;}
    [EmailAddress (ErrorMessage ="Please enter valid email address")]
    public string Email{get; set;}
    [Required (ErrorMessage ="Eter address")]
    
    [StringLength(400)]
    public string Address{get; set;}
[DateTime]
    public DateTime Admission {get; set;}

}
