using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTO;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly CollegeDBContext _context;

    public StudentController(CollegeDBContext context)
    {
        _context = context;
    }

    // ===================== GET ALL =====================
    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetStudents()
    {
        try
        {
            var students = await _context.Students
                .AsNoTracking()
                .Select(s => new StudentDTO
                {
                    id = s.Id,
                    StudentName = s.StudentName,
                    Email = s.Email,
                    Address = s.Address,
                    MobileNumber = s.MobileNumber,
                    Admission = s.Admission
                })
                .ToListAsync();

            return Ok(students);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Server error: {ex.Message}");
        }
    }

    // ===================== GET BY ID =====================
    [HttpGet("{id:int}/ById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentById(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid student ID.");

        var student = await _context.Students.FindAsync(id);

        if (student == null)
            return NotFound($"Student with ID {id} not found.");

        return Ok(student);
    }

    // ===================== CREATE =====================
    [HttpPost("Create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateStudent([FromBody] StudentDTO model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await _context.Students.AnyAsync(s => s.Email == model.Email))
            return Conflict("Email already exists.");

        var student = new Students
        {
            StudentName = model.StudentName,
            Email = model.Email,
            Address = model.Address,
            MobileNumber = model.MobileNumber,
            Admission = model.Admission
        };

        try
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Database error while saving student.");
        }

        model.id = student.Id;

        return CreatedAtAction(nameof(GetStudentById),
            new { id = student.Id }, model);
    }

    // ===================== UPDATE =====================
    [HttpPut("Update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStudent([FromBody] StudentDTO model)
    {
        if (!ModelState.IsValid || model.id <= 0)
            return BadRequest("Invalid input data.");

        var student = await _context.Students.FindAsync(model.id);

        if (student == null)
            return NotFound($"Student with ID {model.id} not found.");

        if (await _context.Students.AnyAsync(s =>
                s.Email == model.Email && s.Id != model.id))
            return Conflict("Email already exists for another student.");

        student.StudentName = model.StudentName;
        student.Email = model.Email;
        student.Address = model.Address;
        student.MobileNumber = model.MobileNumber;
        student.Admission = model.Admission;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // ===================== DELETE =====================
    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid student ID.");

        var student = await _context.Students.FindAsync(id);

        if (student == null)
            return NotFound($"Student with ID {id} not found.");

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // ===================== PATCH =====================
    [HttpPatch("{id:int}/UpdatePartial")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStudentPartial(
        int id,
        [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
    {
        if (patchDocument == null || id <= 0)
            return BadRequest("Invalid patch request.");

        var student = await _context.Students.FindAsync(id);

        if (student == null)
            return NotFound($"Student with ID {id} not found.");

        var studentDTO = new StudentDTO
        {
            id = student.Id,
            StudentName = student.StudentName,
            Email = student.Email,
            Address = student.Address,
            MobileNumber = student.MobileNumber,
            Admission = student.Admission
        };

        patchDocument.ApplyTo(studentDTO, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        student.StudentName = studentDTO.StudentName;
        student.Email = studentDTO.Email;
        student.Address = studentDTO.Address;
        student.MobileNumber = studentDTO.MobileNumber;
        student.Admission = studentDTO.Admission;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}
