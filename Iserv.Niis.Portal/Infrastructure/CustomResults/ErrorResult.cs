using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Portal.Infrastructure.CustomResults
{
    public class ErrorResult : ObjectResult
    {
        public ErrorResult(string errorMessage = "", int? statusCode = StatusCodes.Status400BadRequest) : base(new {error = errorMessage})
        {
            StatusCode = statusCode;
        }
    }
}
