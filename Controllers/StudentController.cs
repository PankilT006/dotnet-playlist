using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data.Repository;
using WebApi.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using WebApi.Data.Respository;
using Microsoft.AspNetCore.Cors;

namespace WebApi.Controllers
{

    [Route("api/[controller]")]
    // Enable CORS for this controller
    [ApiController]

    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _repository;
        private readonly IMapper _mapper;


        public StudentController(IStudentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // ===================== GET ALL =====================
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _repository.GetAllAsync();

            if (students == null || !students.Any())
                return NotFound("No students found.");

            return Ok(_mapper.Map<List<StudentDTO>>(students));
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

            var student = await _repository.GetByIdAsync(student => student.Id == id);

            if (student == null)
                return NotFound($"Student with ID {id} not found.");

            return Ok(_mapper.Map<StudentDTO>(student));
        }

        // ===================== CREATE =====================
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateStudent([FromBody] StudentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var student = _mapper.Map<Students>(dto);

            var studentAfterCreation = await _repository.CreateAsync(student);

            dto.id = studentAfterCreation.Id;

            return CreatedAtAction(nameof(GetStudentById),
                new { id = dto.id }, dto);
        }

        // ===================== UPDATE =====================
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStudent([FromBody] StudentDTO dto)
        {
            if (!ModelState.IsValid || dto.id <= 0)
                return BadRequest("Invalid input data.");

            var student = _mapper.Map<Students>(dto);

            var studentAfterUpdated = await _repository.UpdateAsync(student);

            if (studentAfterUpdated == null)
                return NotFound($"Student with ID {dto.id} not found.");

            return NoContent();
        }

        // ===================== DELETE =====================
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid student ID.");

            var deleted = await _repository.GetByIdAsync(student => student.Id == id);
            // Console.WriteLine(deleted.Id);
            if (deleted == null)
                return NotFound($"Student with ID {id} not found.");
            await _repository.DeleteAsync(deleted);

            return Ok("Student deleted successfully.");
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

            var student = await _repository.GetByIdAsync(s => s.Id == id);

            if (student == null)
                return NotFound($"Student with ID {id} not found.");

            var studentDTO = _mapper.Map<StudentDTO>(student);

            patchDocument.ApplyTo(studentDTO, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedStudent = _mapper.Map<Students>(studentDTO);
            updatedStudent.Id = id;

            var studentAfterPartialUpdate = await _repository.UpdateAsync(updatedStudent);

            if (studentAfterPartialUpdate == null)
                return NotFound($"Student with ID {id} not found.");

            return NoContent();
        }
    }
}
