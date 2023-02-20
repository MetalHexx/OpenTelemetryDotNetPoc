using GatewayApi.Telemetry.Tracing;
using Microsoft.AspNetCore.Mvc;

namespace GatewayApi.Features.Weather
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherService;
        private readonly IActivityService _activityService;
        private const string _className = nameof(WeatherForecastController);

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService, IActivityService activityService)
        {
            _logger = logger;
            _weatherService = weatherService;
            _activityService = activityService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get()
        {
            using var activity = _activityService.StartActivity(_className, nameof(Get), "Getting weather from service");
            
            var forecast = _weatherService.GetWeatherForecast();
            return Ok(forecast);
        }
    }
}