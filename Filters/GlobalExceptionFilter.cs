using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClinicWebApi.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
       

        public override void OnException(ExceptionContext context)
        {

            var userName = context.HttpContext.User?.FindFirst("UserName")?.Value;


            if (string.IsNullOrEmpty(userName))
            {
                userName = null;
            }

            var result = new ObjectResult("System Error Try Again")
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            var error = context.Exception.Message;

            //try
            //{
            //    package შევქმნა და გავიტანო ერორი ცხრილში
            //}
            //catch
            //{

            //}

            context.Result = result;
        }
    }
}

