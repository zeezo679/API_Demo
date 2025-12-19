using ApiProj.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ApiProj.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
       

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration; //bad design approach (no ISP or SOC)
        private readonly WeatherAppOptions _weatherOptions;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpClientFactory httpClientFactory, IConfiguration configuration, IOptionsSnapshot<WeatherAppOptions> weatherOptions)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _weatherOptions = weatherOptions.Value;
        }

        [HttpGet]
        public async Task<String> Get(string cityName = "Egypt")
        {
            string? baseUrl = _weatherOptions.BaseUrl;
            string? key = _weatherOptions.Key;

            string url = $"{baseUrl}.json?key={key}&q={cityName}&aqi=no";

            using var client = _httpClientFactory.CreateClient();

            return await client.GetStringAsync(url);
        }
    }
}
