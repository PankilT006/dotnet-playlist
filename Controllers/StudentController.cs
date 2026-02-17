using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data.Repository;
using WebApi.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetStudents()
        {
            var students = await _repository.GetStudentsAsync();

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

            var student = await _repository.GetStudentByIdAsync(id);

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

            var id = await _repository.CreateStudentAsync(student);

            dto.id = id;

            return CreatedAtAction(nameof(GetStudentById),
                new { id = id }, dto);
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

            var updated = await _repository.UpdateStudentAsync(student);

            if (!updated)
                return NotFound($"Student with ID {dto.id} not found.");

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

            var deleted = await _repository.DeleteStudentAsync(id);

            if (!deleted)
                return NotFound($"Student with ID {id} not found.");

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

            var student = await _repository.GetStudentByIdAsync(id);

            if (student == null)
                return NotFound($"Student with ID {id} not found.");

            var studentDTO = _mapper.Map<StudentDTO>(student);

            patchDocument.ApplyTo(studentDTO, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedStudent = _mapper.Map<Students>(studentDTO);
            updatedStudent.Id = id;

            var updated = await _repository.UpdateStudentAsync(updatedStudent);

            if (!updated)
                return NotFound($"Student with ID {id} not found.");

            return NoContent();
        }
    }
}
