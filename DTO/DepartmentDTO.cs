using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO;

public class DepartmentDTO
{

    public int id { get; set; }

    [Required(ErrorMessage = "Enter Department name")]
    public required string DepartmentName { get; set; }
    public string? Description { get; set; }

    
}
