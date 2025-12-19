using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ApiProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IServer _server;

        public TestController(IServer server)
        {
            _server = server;
        }


        [HttpGet("ping")]
        public IActionResult Ping()
        {
            // Get the current process information (e.g., w3wp, iisexpress, dotnet)
            var proc = Process.GetCurrentProcess();
            // Return hosting environment details as JSON response
            return Ok(new
            {
                message = "Hosting Info",               // Custom message
                pid = Environment.ProcessId,            // Numeric process ID
                process = proc.ProcessName,             // Running process name
                serverType = _server.GetType().Name,    // Hosting server type (IISHttpServer or KestrelServer)
                machine = Environment.MachineName       // Server machine name
            });
        }
    }
}
