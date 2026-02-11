using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WebApi.Validators;

namespace WebApi.Data;
[Index(nameof(Email),IsUnique =true)]
public class Students
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public required string StudentName { get; set; }
    [EmailAddress]
    [StringLength(255)]
    
    public required string Email { get; set; }

    public required string Address { get; set; }
     public required string? MobileNumber { get; set; }
    public DateTime Admission { get; set; }
   
}
