using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProj.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController] //for auto model binding and validation and cleaner error handling
    public class EmployeeController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hello";
        }

        [HttpGet]
        public string GetEmployee()
        {
            return "Hello";
        }
    }
}
