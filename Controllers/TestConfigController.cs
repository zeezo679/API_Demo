using ApiProj.Models;
using ApiProj.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;




namespace ApiProj.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestConfigController : ControllerBase
    {
        private IConfiguration _configuration;
        private IOptions<ConnectionStrings> _connectionStringOptions;
        public TestConfigController(IConfiguration configuration, IOptions<ConnectionStrings> connectionStringOptions)
        {
            _configuration = configuration;
            _connectionStringOptions = connectionStringOptions;
        }

        [HttpGet]
        public IActionResult GetConfigValue()
        {
            var appName = _configuration.GetSection("MyAppSettings:ApplicationName").Value;
            var version = _configuration["MyAppSettings:Version"];
            var allSettings = new MyAppsettings();

            _configuration.GetSection("MyAppSettings").Bind(allSettings);

            //property names must match appsettings.json
            var data = new
            {
                ApplicationName = appName,
                Version = version,
            };
            return Ok(data);
        }

        [HttpGet]
        public IActionResult TestConnValue()
        {
            var defaultConn = _connectionStringOptions.Value.DefaultConnection;
            return Ok(defaultConn);
        }
    }
}
