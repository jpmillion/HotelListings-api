using HotelListingsApi.Dto.Errors;
using HotelListingsApi.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace HotelListingsApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in {context.Request.Path}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode;
            ErrorDetails details = new ErrorDetails();

            switch (ex)
            {
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    details.ErrorType = "Not Found";
                    details.ErrorMessage = ex.Message;
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    details.ErrorType = "Failure";
                    details.ErrorMessage = ex.Message;
                    break;
            }

            string response = JsonConvert.SerializeObject(details);
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(response);
        }
    }
}
