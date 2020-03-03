using System;
using FluentValidation;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.Portal.Infrastructure.CustomResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Iserv.Niis.Portal.Infrastructure.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var contextException = context.Exception;
            var contextExceptionMessage = $"{Guid.NewGuid()}: {contextException.Message}";

            Log.Warning(contextException, contextExceptionMessage);

            switch (contextException)
            {
                case DictionaryNameException _:
                    context.Result = new ErrorResult(contextExceptionMessage);
                    break;
                case SecurityException _:
                    context.Result = new ErrorResult(contextExceptionMessage, StatusCodes.Status401Unauthorized);
                    break;
                case FilterException _:
                case ValidationException _:
                    context.Result =
                        new ErrorResult(contextExceptionMessage, StatusCodes.Status422UnprocessableEntity);
                    break;
                case DataNotFoundException _:
                    context.Result = new ErrorResult(contextExceptionMessage, StatusCodes.Status404NotFound);
                    break;
                case DatabaseException _:
                    context.Result =
                        new ErrorResult(contextExceptionMessage, StatusCodes.Status500InternalServerError);
                    break;
                default:
                    context.Result =
                        new ErrorResult(contextExceptionMessage, StatusCodes.Status500InternalServerError);
                    break;
            }

            context.ExceptionHandled = true;
        }
    }
}