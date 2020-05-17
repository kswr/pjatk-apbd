using Microsoft.AspNetCore.Mvc;
using WebApplication.DAL;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IDbService _dbService;

        public EnrollmentController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public IActionResult Enroll(EnrollmentRequest request)
        {
            return Ok(_dbService.EnrollStudent(request));
        }
    }
}