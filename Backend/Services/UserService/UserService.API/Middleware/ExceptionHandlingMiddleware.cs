﻿using System.Text.Json;
using UserService.BusinessLogic.Exceptions;

namespace UserService.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BadRequestException ex)
            {
                await HandleException(context, ex);
            }
            catch (NotFoundException ex)
            {
                await HandleException(context, ex);
            }
            catch (UnauthorizedException ex)
            {
                await HandleException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }

        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            string message = exception.Message;
            string? details = (exception as CustomException)?.Details;
            int statusCode = exception switch
            {
                BadRequestException => 400,
                NotFoundException => 404,
                UnauthorizedException => 401,
                _ => 500
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                StatusCode = statusCode,
                Message = message,
                Details = details
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
