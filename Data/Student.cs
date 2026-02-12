using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using WebApi.Validators;

namespace WebApi.Data;
public class Students
{
    public int Id { get; set; }

    public required string StudentName { get; set; }

    public required string Email { get; set; }

    public required string Address { get; set; }
    public required string? MobileNumber { get; set; }
    [DateTime]
    public DateTime Admission { get; set; }

}
