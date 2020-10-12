using System.Collections.Generic;

namespace Application.Quota.Dtos
{
    public class SimulateQuotasResponse
    {
        public decimal AvailableSpaceSimulated { get; set; }
        public List<QuotaDataDto> Quotas { get; set; }
        public decimal TotalCreditValue { get; set; }
    }
}
