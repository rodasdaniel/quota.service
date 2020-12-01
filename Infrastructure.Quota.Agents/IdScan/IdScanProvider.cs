using Application.Quota.Dtos;
using Infrastructure.Quota.Agents.CallService;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Quota.Agents.IdScan
{
    public class IdScanProvider : IIdScanProvider
    {
        #region Constructor
        private readonly IConfiguration _config;

        public IdScanProvider(IConfiguration config)
        {
            _config = config;
        }
        #endregion 
        public async Task<IdScanResponse> Scan(IdScanRequest idScanRequest)
        {
            IdScanResponse response = (IdScanResponse)
                CallRestService.CallServiceAsync<IdScanResponse>(
                    _config.GetSection("AgentEndpoints:IdScan").Value,
                        idScanRequest, Method.POST, idScanRequest.id, false, false).Result;
            if (response == null) return null;
            return response;
        }
    }
}
