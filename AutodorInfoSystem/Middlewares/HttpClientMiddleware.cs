using AutodorInfoSystem.Services;

namespace AutodorInfoSystem.Middlewares
{
    public class HttpClientMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpClientService _httpClientService;

        public HttpClientMiddleware(RequestDelegate next, HttpClientService httpClientService)
        {
            _next = next;
            _httpClientService = httpClientService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Получаем токен из куки
            var token = context.Request.Cookies["A"];

            // Устанавливаем авторизацию
            _httpClientService.SetAuthorization(token);

            // Вызываем следующий middleware в конвейере
            await _next(context);
        }
    }
}
