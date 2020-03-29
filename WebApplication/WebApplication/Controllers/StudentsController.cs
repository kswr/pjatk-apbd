using Microsoft.AspNetCore.Mvc;
using WebApplication.DAL;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route(("api/students"))]
    public class StudentsController : ControllerBase
    {

        private readonly IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            Student student = _dbService.GetStudent(id);
            if (student != null) return Ok(student);
            return NotFound("Nie znaleziono studenta");
        }

        [HttpPost]
        public IActionResult PutStudent(Student student)
        {
            _dbService.PutStudent(student);
            return Ok(student);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            _dbService.DeleteStudent(id);
            return Ok("Usuwanie ukończone");
        }
    }
}