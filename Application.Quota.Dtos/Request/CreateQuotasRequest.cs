using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Quota.Dtos
{
    public class CreateQuotasRequest
    {
        [Required(ErrorMessage = "The quotas are required."),
            ListValidation(ErrorMessage = "The quotas format are not valid, you must send at least one quota.")]
        public List<QuotaDataDto> Quotas { get; set; }
        [Required(ErrorMessage = "The client ID is required."),
            RegularExpression(@"^[0-9]*$", ErrorMessage = "The client ID format is not valid."),
            Range(1, long.MaxValue, ErrorMessage = "The value of the client ID is outside the allowed range.")]
        public long IdClient { get; set; }
    }
}
