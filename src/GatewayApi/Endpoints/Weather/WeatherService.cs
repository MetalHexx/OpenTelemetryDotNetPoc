using GatewayApi.Telemetry.Tracing;


namespace GatewayApi.Features.Weather
{
    public class WeatherService : IWeatherService
    {
        private static readonly string[] _summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly IActivityService _activityService;
        private const string _className = nameof(WeatherService);

        public WeatherService(IActivityService activityService)
        {
            _activityService = activityService;
        }

        public WeatherForecast[] GetWeatherForecast()
        {
            using var activity = _activityService.StartActivity(
                new(_className, nameof(GetWeatherForecast), "Generating weather data"));

            var weather = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = _summaries[Random.Shared.Next(_summaries.Length)]
            })
            .ToArray();

            return weather;
        }         
    }
}
