﻿using AskIt.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace AskIt.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            int statusCode;

            if (exception is NotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
            }
            else if (exception is InvalidArgumentException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (exception is NotAllowedException)
            {
                statusCode = (int)HttpStatusCode.Forbidden;
            }
            else if (exception is NotAuthenticatedException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                statusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.Result = new ObjectResult("Error message: " + exception.Message) { StatusCode = statusCode };
        }
    }
}
