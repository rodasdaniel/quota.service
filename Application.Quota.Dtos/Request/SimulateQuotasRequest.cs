using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Quota.Dtos
{
    public class SimulateQuotasRequest
    {
        [Required(ErrorMessage = "The frequency is required."),
            RegularExpression(@"^[0-9]*$", ErrorMessage = "The frequency format is not valid."),
            Range(1, int.MaxValue, ErrorMessage = "The value of the frequency is outside the allowed range."),
            FrequencyValidation(ErrorMessage = "The frequency format is not valid (15 - 30).")]
        public int Frequency { get; set; }
        [Required(ErrorMessage = "The capital value is required."),
            RegularExpression(@"^[0-9]*$", ErrorMessage = "The capital value format is not valid."),
            Range(1, double.MaxValue, ErrorMessage = "The capital value is outside the allowed range.")]
        public decimal CapitalValue { get; set; }
        [Required(ErrorMessage = "The term months value is required."),
            RegularExpression(@"^[0-9]*$", ErrorMessage = "The term months value format is not valid."),
            Range(1, int.MaxValue, ErrorMessage = "The value of the term months value is outside the allowed range."),
            TermMonthsValidation(ErrorMessage = "The term months value format is not valid (2 - 4 - 6 - 12).")]
        public int TermMonths { get; set; }
        [Required(ErrorMessage = "The client ID is required."),
            RegularExpression(@"^[0-9]*$", ErrorMessage = "The client ID format is not valid."),
            Range(1, long.MaxValue, ErrorMessage = "The value of the client ID is outside the allowed range.")]
        public long IdClient { get; set; }

    }
}
