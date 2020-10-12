using Application.Quota.Business.Quota;
using Application.Quota.Common.Utils;
using Application.Quota.Dtos;
using Application.Quota.Dtos.Common;
using AutoMapper;
using Domain.Quota.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Application.Quota.Common.Resources.Messages;

namespace Quota.Service.Test
{
    public class QuotaBusinessTest : IQuotaBusiness
    {
        #region Constructor
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private List<QuotaEntity> quotasMock = new List<QuotaEntity>();
        private List<CreditEntity> creditsMock = new List<CreditEntity>();
        private InfoClientDto infoClientMock = new InfoClientDto
        {
            IdClient = 1,
            TotalSpace = 2000000,
            AvailableSpace = 2000000
        };
        public QuotaBusinessTest(
            IMapper mapper,
            IConfiguration config)
        {
            _mapper = mapper;
            _config = config;
            for (int i = 1; i <= 5; i++)
            {
                quotasMock.Add(new QuotaEntity
                {
                    IdCuota = i,
                    IdCredito = 1,
                    ValorCapital = 22500,
                    ValorTotal = 23625,
                    FechaPago = DateTime.Now
                });
            }
            creditsMock.Add(new CreditEntity { IdCliente = 1, IdCredito = 1 });
        }
        #endregion
        public async Task<(HttpStatusCode statusCode, string message, bool response)>
            CreateQuotas(CreateQuotasRequest createQuotasRequest)
        {
            InfoClientDto infoClientDto = null;
            (bool isValid, HttpStatusCode validationStatusCode, string validationMessage) =
                CreateQuotaValidations(createQuotasRequest, ref infoClientDto);
            if (!isValid)
            {
                return (validationStatusCode, validationMessage, false);
            }
            List<QuotaEntity> quotas = _mapper.Map<List<QuotaEntity>>(createQuotasRequest.Quotas);
            return (HttpStatusCode.OK, SuccessMsg, true);
        }

        public async Task<(HttpStatusCode statusCode, string message, List<QuotaDataDto> response)>
            GetByCredit(long idCredit)
        {
            List<QuotaEntity> quotas = quotasMock.Where(c => c.IdCredito == idCredit).ToList();
            if (quotas == null || quotas.Count <= 0)
            {
                return (HttpStatusCode.NoContent, NoQuotasExistMsg, null);
            }
            else
            {
                List<QuotaDataDto> quotasDto = new List<QuotaDataDto>();
                foreach (QuotaEntity q in quotas)
                {
                    quotasDto.Add(new QuotaDataDto
                    {
                        IdQuota = q.IdCuota,
                        IdCredit = q.IdCredito,
                        CapitalValue = q.ValorCapital,
                        TotalValue = q.ValorTotal,
                        PaymentDate = q.FechaPago
                    });
                }
                return (HttpStatusCode.OK, SuccessMsg, quotasDto);
            }
        }

        public async Task<(HttpStatusCode statusCode, string message, SimulateQuotasResponse response)>
            SimulateQuotas(SimulateQuotasRequest simulateQuotasRequest)
        {
            InfoClientDto infoClientDto = null;
            (bool isValid, HttpStatusCode validationStatusCode, string validationMessage) =
                SimulateQuotaValidations(simulateQuotasRequest, ref infoClientDto);
            if (!isValid)
            {
                return (validationStatusCode, validationMessage, null);
            }
            return (HttpStatusCode.OK, SuccessMsg,
                GetResponse(infoClientDto, simulateQuotasRequest));
        }
        #region Private
        private SimulateQuotasResponse GetResponse(InfoClientDto infoClientDto,
            SimulateQuotasRequest simulateQuotasRequest)
        {
            List<QuotaDataDto> quotas = CalculateQuotas(simulateQuotasRequest);
            return new SimulateQuotasResponse
            {
                AvailableSpaceSimulated = infoClientDto.AvailableSpace - simulateQuotasRequest.CapitalValue,
                Quotas = quotas,
                TotalCreditValue = quotas.Sum(c => c.TotalValue)
            };
        }
        private List<QuotaDataDto> CalculateQuotas(SimulateQuotasRequest simulateQuotasRequest)
        {
            List<QuotaDataDto> reponse = new List<QuotaDataDto>();
            int quotaQuantity = simulateQuotasRequest.TermMonths *
                ((simulateQuotasRequest.Frequency == 15) ? 2 : 1);
            decimal monthRate = decimal.Parse(_config.GetSection("CommonValues:MonthlyRate").Value);
            decimal totalQuotaValue = (((simulateQuotasRequest.CapitalValue * monthRate) *
                simulateQuotasRequest.TermMonths) + simulateQuotasRequest.CapitalValue) / quotaQuantity;
            decimal quotaCapitalValue = simulateQuotasRequest.CapitalValue / quotaQuantity;
            for (int i = 1; i <= quotaQuantity; i++)
            {
                reponse.Add(new QuotaDataDto
                {
                    IdQuota = i,
                    CapitalValue = quotaCapitalValue,
                    TotalValue = totalQuotaValue,
                    PaymentDate =
                        ColombianHour.GetDate().
                        AddDays(((simulateQuotasRequest.Frequency == 15) ? 15 : 30) * i)
                });
            }
            return reponse;
        }
        private (bool IsValid, HttpStatusCode statusCode, string message)
            ClientValidations(long idClient, decimal capitalValue, ref InfoClientDto infoClientDto)
        {
            infoClientDto = infoClientMock;
            if (infoClientDto == null)
            {
                return (false, HttpStatusCode.BadRequest, ClientNoExistMsg);
            }
            if (infoClientDto.AvailableSpace < capitalValue)
            {
                return (false, HttpStatusCode.PreconditionFailed, NoAvalibleSpaceMsg);
            }
            return (true, HttpStatusCode.OK, "");
        }
        private (bool IsValid, HttpStatusCode statusCode, string message)
            CreateQuotaValidations(CreateQuotasRequest createQuotasRequest, ref InfoClientDto infoClientDto)
        {
            (bool clientIsValid, HttpStatusCode clientatusCode, string clientMessage) =
                ClientValidations(createQuotasRequest.IdClient, createQuotasRequest.Quotas.Sum(c => c.CapitalValue),
                ref infoClientDto);
            if (!clientIsValid)
            {
                return (false, clientatusCode, clientMessage);
            }
            long idCredit = createQuotasRequest.Quotas.FirstOrDefault().IdCredit;
            if (createQuotasRequest.Quotas.Count > 1)
            {
                foreach (QuotaDataDto quota in createQuotasRequest.Quotas)
                {
                    if (quota.IdCredit != idCredit) return (false, HttpStatusCode.BadRequest, IdCreditError);
                }
            }
            if (!(creditsMock.Where(c => c.IdCliente == createQuotasRequest.IdClient 
                && c.IdCredito == idCredit).ToList().Count > 0))
            {
                return (false, HttpStatusCode.BadRequest, CreditNoExist);
            }
            return (true, HttpStatusCode.OK, "");
        }
        private (bool IsValid, HttpStatusCode statusCode, string message)
            SimulateQuotaValidations(SimulateQuotasRequest simulateQuotasRequest, ref InfoClientDto infoClientDto)
        {
            (bool clientIsValid, HttpStatusCode clientatusCode, string clientMessage) =
                ClientValidations(simulateQuotasRequest.IdClient, simulateQuotasRequest.CapitalValue,
                ref infoClientDto);
            if (!clientIsValid)
            {
                return (false, clientatusCode, clientMessage);
            }
            if (!ValidateTermMonths(simulateQuotasRequest))
            {
                return (false, HttpStatusCode.BadRequest, TermNoAllowedMsg);
            }
            return (true, HttpStatusCode.OK, "");
        }
        private bool ValidateTermMonths(SimulateQuotasRequest simulateQuotasRequest)
        {
            List<TermDto> termsDto = _config.GetSection("CommonValues:Terms").Get<List<TermDto>>();
            List<int> allowedTerms = new List<int>();
            foreach (TermDto term in termsDto)
            {
                if ((simulateQuotasRequest.CapitalValue >= term.From
                    && simulateQuotasRequest.CapitalValue <= term.To)
                    || simulateQuotasRequest.CapitalValue > term.To)
                {
                    allowedTerms.Add(term.Months);
                }
            }
            return allowedTerms.Contains(simulateQuotasRequest.TermMonths);
        }
        #endregion 
    }
}
