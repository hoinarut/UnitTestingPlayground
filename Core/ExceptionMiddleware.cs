using Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Core
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException mve)
            {
                _logger.LogError(mve, "Validation Exception");
                await HandleValidationExceptionAsync(httpContext, mve);
            }
            catch (EntityNotFoundException enfe)
            {
                _logger.LogError(enfe, "Entity Not Found - See details");
                await HandleEntityNotFoundExceptionAsync(httpContext, enfe);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleEntityNotFoundExceptionAsync(HttpContext context, EntityNotFoundException enfe)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            return context.Response.WriteAsync("The resource you are looking for does not exist in this dimension");
        }

        private Task HandleValidationExceptionAsync(HttpContext context, ValidationException mve)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            if (mve.ValidationErrors != null && mve.ValidationErrors.Count != 0)
            {
                return context.Response.WriteAsync(JsonConvert.SerializeObject(mve.ValidationErrors));
            }
            else
            {
                return context.Response.WriteAsync(mve.Message);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync("Something is not right here .. you should probably check the logs!");
        }
    }
}
