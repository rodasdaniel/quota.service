using Application.Quota.Dtos;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Application.Quota.Business.Quota
{
    public interface IQuotaBusiness
    {
        Task<(HttpStatusCode statusCode, string message, List<QuotaDataDto> response)>
            GetByCredit(long idCredit);
        Task<(HttpStatusCode statusCode, string message, SimulateQuotasResponse response)>
            SimulateQuotas(SimulateQuotasRequest simulateQuotasRequest);
        Task<(HttpStatusCode statusCode, string message, bool response)>
            CreateQuotas(CreateQuotasRequest createQuotasRequest);
    }
}
