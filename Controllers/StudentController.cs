using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.DTO;
using WebApi.Controllers.Models;
using WebApi.Controllers.Repository;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]

public class StudentController : ControllerBase
{
    [HttpGet("All")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public ActionResult<IEnumerable<StudentDTO>> GetStudents()
    {
        var students = StudentRepository.Students.Select(s => new StudentDTO()
        {
            id = s.id,
            StudentName = s.StudentName,
            Address = s.Address,
            Email = s.Email,
            Admission = s.Admission
        });
        //Ok- 200 status code sucess
        return Ok(students);
    }
    [HttpGet("{id:int}/ById")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public ActionResult<StudentDTO> GetStudentById(int id)
    {

        if (id <= 0)
            //400 client error
            return BadRequest();

        var std = StudentRepository.Students.Where(n => n.id == id).FirstOrDefault();
        if (std == null)
            return NotFound($"Student with id {id} not found");//404 not found msg

        var studentDTO = new StudentDTO
        {
            id = std.id,
            StudentName = std.StudentName,
            Address = std.Address,
            Email = std.Email,
            Admission = std.Admission
        };
        return Ok(studentDTO);
    }
    [HttpGet("{name:alpha}/ByName")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public ActionResult<StudentDTO> GetStudentByName(string name)
    {
        if (string.IsNullOrEmpty(name))
            //400 client error
            return BadRequest();

        var std = StudentRepository.Students.Where(n => n.StudentName == name).FirstOrDefault();
        if (std == null)
            return NotFound($"Student with name {name} not found");//404 not found msg
        var studentDTO = new StudentDTO
        {
            id = std.id,
            StudentName = std.StudentName,
            Address = std.Address,
            Email = std.Email,
            Admission = std.Admission

        };
        return Ok(studentDTO);
    }
    [HttpDelete]
    [Route("Delete/{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<bool> DeleteStudent(int id)

    {
        if (id <= 0)
            //400 client error
            return BadRequest();

        var std = StudentRepository.Students.Where(n => n.id == id).FirstOrDefault();
        if (std == null)
            return NotFound($"Student with id {id} not found");//404 not found msg 
        StudentRepository.Students.Remove(std);
        return true;
    }

    [HttpPost]
    [Route("Create")]
    [ProducesResponseType(200)]
    [ProducesResponseType(201)]
    [ProducesResponseType(404)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO Model)
    {
        if (Model == null)
            return BadRequest();

        int newId = StudentRepository.Students.LastOrDefault().id + 1;

        Student student = new Student
        {
            id = newId,
            StudentName = Model.StudentName,
            Address = Model.Address,
            Email = Model.Email,
            Admission = Model.Admission
        };
        StudentRepository.Students.Add(student);
        Model.id = student.id;
        return Ok(student);
    }

    [HttpPut]
    [Route("Update")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<StudentDTO> UpdateStudent([FromBody] StudentDTO Model)
    {
        if (Model == null || Model.id <= 0)
        {
            return BadRequest();
        }
        var existing = StudentRepository.Students.Where(s => s.id == Model.id).FirstOrDefault();
        if (existing == null)
            return NotFound();
        existing.StudentName = Model.StudentName;
        existing.Email = Model.Email;
        existing.Address = Model.Address;
        existing.Admission = Model.Admission;
        return NoContent();

    }

     [HttpPatch]
    [Route("{id:int}/UpdatePartial")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<StudentDTO> UpdateStudentPartial(int id,[FromBody] JsonPatchDocument<StudentDTO> patchDocument)
    {
        if (patchDocument == null || id <= 0)
        {
            return BadRequest();
        }
        var existing = StudentRepository.Students.Where(s => s.id == id).FirstOrDefault();
        if (existing == null)
            return NotFound();

        var studentDTO = new StudentDTO{
            id=existing.id,
            StudentName=existing.StudentName,
            Email=existing.Email,
            Address=existing.Address,
            Admission=existing.Admission
        };
        patchDocument.ApplyTo(studentDTO,ModelState);
        if(!ModelState.IsValid)
        return BadRequest(ModelState);

        existing.StudentName = studentDTO.StudentName;
        existing.Email = studentDTO.Email;
        existing.Address = studentDTO.Address;
        existing.Admission = studentDTO.Admission;
        return NoContent();

    }
}
