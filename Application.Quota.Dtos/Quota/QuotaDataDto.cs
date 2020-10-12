using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Quota.Dtos
{
    public class QuotaDataDto
    {
        [Required(ErrorMessage = "The quota ID is required."),
            RegularExpression(@"^[0-9]*$", ErrorMessage = "The quota ID format is not valid."),
            Range(1, int.MaxValue, ErrorMessage = "The value of the quota ID is outside the allowed range.")]
        public int IdQuota { get; set; }
        [Required(ErrorMessage = "The credit ID is required."),
            RegularExpression(@"^[0-9]*$", ErrorMessage = "The credit ID format is not valid."),
            Range(1, long.MaxValue, ErrorMessage = "The value of the credit ID is outside the allowed range.")]
        public long IdCredit { get; set; }
        [Required(ErrorMessage = "The capital value is required."),
            RegularExpression(@"^[0-9]*$", ErrorMessage = "The capital value format is not valid."),
            Range(1, double.MaxValue, ErrorMessage = "The capital value is outside the allowed range.")]
        public decimal CapitalValue { get; set; }
        [Required(ErrorMessage = "The total value is required."),
            RegularExpression(@"^[0-9]*$", ErrorMessage = "The total value format is not valid."),
            Range(1, double.MaxValue, ErrorMessage = "The total value is outside the allowed range.")]
        public decimal TotalValue { get; set; }
        [Required(ErrorMessage = "The payment date is required.")]
        public DateTime PaymentDate { get; set; }
    }
}
