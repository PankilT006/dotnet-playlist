using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTO;
using AutoMapper;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly CollegeDBContext _context;
    private readonly IMapper _mapper;
    
    public StudentController(CollegeDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // ===================== GET ALL =====================
    [HttpGet("All")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetStudents()
    {
        try
        {//below line is not recommended as it will load entire entity and then we will map it to DTO in memory which is inefficient and can cause performance issues for large datasets
            var students = await _context.Students.ToListAsync();
            var studentsDTO = _mapper.Map<List<StudentDTO>>(students);
          

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

         var studentDTO = _mapper.Map<StudentDTO>(student);   

        return Ok(studentDTO);
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
            if(await _context.Students.AnyAsync(s => s.MobileNumber == model.MobileNumber))
                return Conflict("Mobile number already exists.");

        var student = _mapper.Map<Students>(model);

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

        // var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == model.id);
        var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == model.id);


        if (student == null)
            return NotFound($"Student with ID {model.id} not found.");

        if (await _context.Students.AnyAsync(s =>
                s.Email == model.Email && s.Id != model.id))
            return Conflict("Email already exists for another student.");

        var student1 = _mapper.Map<Students>(model);
        _context.Students.Update(student1);

        // student.StudentName = model.StudentName;
        // student.Email = model.Email;
        // student.Address = model.Address;
        // student.MobileNumber = model.MobileNumber;
        // student.Admission = model.Admission;

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

        var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);

        if (student == null)
            return NotFound($"Student with ID {id} not found.");

        var studentDTO = _mapper.Map<StudentDTO>(student);

        patchDocument.ApplyTo(studentDTO, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // student.StudentName = studentDTO.StudentName;
        // student.Email = studentDTO.Email;
        // student.Address = studentDTO.Address;
        // student.MobileNumber = studentDTO.MobileNumber;
        // student.Admission = studentDTO.Admission;
             _context.Students.Update(_mapper.Map<Students>(studentDTO));
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
