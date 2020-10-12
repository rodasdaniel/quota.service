using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Application.Quota.Dtos
{
    public static class ServiceAnswer<T>
    {
        public static IActionResult Response(HttpStatusCode code, string message, T response)
        {
            return new ObjectResult(new { statusCode = code })
            {
                Value = new HttpResponseDto<T> { Code = Convert.ToInt32(code), Description = message, Object = response }
            };
        }
    }
}
