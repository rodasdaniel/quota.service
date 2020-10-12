using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using static Application.Quota.Common.Resources.Messages;

namespace Application.Quota.Common.Handler
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;
        public HttpResponseExceptionFilter() { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (IsExecption(context))
                GenerateGenericExeption(context);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        private void GenerateGenericExeption(ActionExecutedContext context)
        {
            context.HttpContext.Response.ContentType = "application/json";
            (int statusCode, string msg) = GetStatusCode(context.Exception.Message);

            context.Result = new ObjectResult(new HttpResponseException
            {
                Code = statusCode,
                Message = (string.IsNullOrEmpty(msg)) ? UnexpectedErrorMsg : msg,
                Detail = ""
            })
            { StatusCode = statusCode };

            context.ExceptionHandled = true;
        }

        #region Private

        private static bool IsExecption(ActionExecutedContext context)
        {
            return context.Exception != null;
        }

        private (int statusCode, string msg) GetStatusCode(string message)
        {
            if (!string.IsNullOrEmpty(message) && message.Contains("StatusCode ["))
            {
                string status = (message.Split('[').Last()).Split(']').First();
                string msg = message.Split(']').Last();
                return (Convert.ToInt32(status), msg);
            }
            return (Convert.ToInt32(500), message);
        }
        #endregion
    }
}
