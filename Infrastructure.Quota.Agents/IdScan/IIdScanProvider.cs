using Application.Quota.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Quota.Agents.IdScan
{
    public interface IIdScanProvider
    {
        Task<IdScanResponse> Scan(IdScanRequest idScanRequest);
    }
}
