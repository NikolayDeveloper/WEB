using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Iserv.Niis.Exceptions;
using Iserv.Niis.Infrastructure.CustomResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Iserv.Niis.Infrastructure.Infrastructure.Filters
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
                case DbUpdateConcurrencyException _:
                case OptimisticConcurrencyException _:
                    context.Result =
                        new ErrorResult(contextExceptionMessage, StatusCodes.Status412PreconditionFailed);
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