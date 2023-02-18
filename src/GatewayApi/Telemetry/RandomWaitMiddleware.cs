namespace GatewayApi.Telemetry
{
    public static class RandomWaitMiddlewareExtensions
    {
        public static IApplicationBuilder UseRandomWaitMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RandomWaitMiddleware>();
        }
    }
    public class RandomWaitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Random _random = new Random();

        public RandomWaitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            int waitTime = _random.Next(0, 1000);
            await Task.Delay(waitTime);

            await _next(context);
        }
    }
}
