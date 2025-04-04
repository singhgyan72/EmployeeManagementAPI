﻿using EmployeeManagementAPI.Contracts;
using EmployeeManagementAPI.Entities.ErrorModel;
using EmployeeManagementAPI.Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace EmployeeManagementAPI.Host
{
    //In .NET 8, we can use this new interface to globally handle exceptions in our project.
    //The IExceptionHandler interface has a single method member named TryHandleAsync
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILoggerManager _logger;

        public GlobalExceptionHandler(ILoggerManager logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.ContentType = "application/json";
            var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
            if (contextFeature != null)
            {
                httpContext.Response.StatusCode = contextFeature.Error switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };
                _logger.LogError($"Something went wrong: {exception.Message}");
                await httpContext.Response.WriteAsync(new ErrorDetails()
                {
                    StatusCode = httpContext.Response.StatusCode,
                    Message = "Internal Server Error.",
                }.ToString());
            }
            return true;
        }
    }
}
