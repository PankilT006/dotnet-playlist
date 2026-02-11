using System.ComponentModel.DataAnnotations;
using WebApi.Validators;

namespace WebApi.DTO;

public class StudentDTO
{
    public int id { get; set; }

    [Required(ErrorMessage = "Enter Student name")]
    [StringLength(70)]
    public required string StudentName { get; set; }

    [Required(ErrorMessage = "Enter email")]
    [EmailAddress(ErrorMessage = "Please enter valid email address")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Enter address")]
    [StringLength(400)]
    public required string Address { get; set; }
    [Required]
[StringLength(15)]
public required string MobileNumber { get; set; }

[DateTime]
    public DateTime Admission { get; set; }
}
