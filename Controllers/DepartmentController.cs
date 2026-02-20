using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data.Repository;
using WebApi.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
        [EnableCors(PolicyName ="AllowSpecificOrigins")]
        [Authorize(Roles = "Admin,User")]

    public class DepartmentController : ControllerBase
    {
          private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;


        public DepartmentController(IDepartmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // ===================== GET ALL =====================
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]


        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllDepartments()
        {   
            var departments = await _repository.GetAllAsync();

            if (departments == null || !departments.Any())
                return NotFound("No departments found.");

            return Ok(_mapper.Map<List<DepartmentDTO>>(departments));
        }

          // ===================== CREATE =====================
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var department = _mapper.Map<Department>(dto);

            var departmentntAfterCreation = await _repository.CreateAsync(department);

            dto.id = departmentntAfterCreation.Id;

            return CreatedAtAction(nameof(GetDepartmentById),
                new { id = dto.id }, dto);
        }


         // ===================== GET BY ID =====================
        [HttpGet("{id:int}/ById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid department ID.");

            var department = await _repository.GetByIdAsync(department => department.Id == id);

            if (department == null)
                return NotFound($"Department with ID {id} not found.");

            return Ok(_mapper.Map<DepartmentDTO>(department));
        }
    }
}
