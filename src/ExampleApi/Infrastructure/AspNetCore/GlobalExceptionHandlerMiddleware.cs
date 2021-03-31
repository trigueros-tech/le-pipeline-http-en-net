using System;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ExampleApi.Infrastructure.AspNetCore
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                switch (e)
                {
                    // NDR : Cela peut être interressant de suivre la norme "Problem Details for HTTP APIs"  https://tools.ietf.org/html/rfc7807
                    case ValidationException validationException:
                        // Bad request
                        context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(new
                        {
                            Errors = validationException.Errors
                        });
                        break;
                    
                    // ...
                    
                    default:
                        // Tout le reste
                        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        break;
                }
            }
        }
    }
}