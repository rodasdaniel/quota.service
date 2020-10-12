using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Quota.API.Filters.ValidateModel
{
    public class ValidationResultModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<ValidationError> Detail { get; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            Code = (int)HttpStatusCode.BadRequest;
            Message = "";
            Detail = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
        }
    }
    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Attribute { get; }

        public string Menssage { get; }

        public ValidationError(string field, string message)
        {
            Attribute = field != string.Empty ? field : null;
            Menssage = message;
        }
    }
}
