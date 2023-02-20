namespace GatewayApi.Features.Weather
{
    public interface IWeatherService
    {
        WeatherForecast[] GetWeatherForecast();
    }
}
