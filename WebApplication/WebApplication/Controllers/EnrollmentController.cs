using Microsoft.AspNetCore.Mvc;
using WebApplication.DTO.Requests;
using WebApplication.Models;
using WebApplication.Services;

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
            var result = _dbService.EnrollStudent(request);
            if (!result.Equals(-1))
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}