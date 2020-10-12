using Application.Quota.Dtos;
using Infrastructure.Quota.Agents.CallService;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Quota.Agents.Client
{
    public class ClientProvider : IClientProvider
    {
        #region Constructor
        private readonly IConfiguration _config;

        public ClientProvider(IConfiguration config)
        {
            _config = config;
        }
        #endregion 
        public async Task<InfoClientDto> GetInfoClient(long idClient)
        {
            HttpResponseDto<InfoClientDto> infoClientDto = (HttpResponseDto<InfoClientDto>)
                CallRestService.CallServiceAsync<HttpResponseDto<InfoClientDto>>(
                    string.Format(_config.GetSection("AgentEndpoints:GetInfoClient").Value, idClient)
                    , null, Method.GET, false, false).Result;
            if (infoClientDto == null) return null;
            return infoClientDto.Object;
        }
    }
}
