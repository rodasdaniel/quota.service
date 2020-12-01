using Application.Quota.Dtos;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Application.Quota.Business.IdScan
{
    public interface IIdScanBusiness
    {
        Task<(HttpStatusCode statusCode, string message, IdScanResponse response)>
            Scan(IdScanRequest idScanRequest);
        Task<(HttpStatusCode statusCode, string message, bool response)>
            MasiveScan();
    }
}
