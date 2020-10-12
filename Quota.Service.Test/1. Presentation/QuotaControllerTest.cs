using Application.Quota.Business.Quota;
using Application.Quota.Dtos;
using AutoMapper;
using Quota.API.App_Start;
using Quota.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.IO;

namespace Quota.Service.Test
{
    public class QuotaControllerTest
    {

        #region Constructor

        QuotaController _controller;
        IQuotaBusiness _service;


        public QuotaControllerTest()
        {

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = mockMapper.CreateMapper();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            _service = new QuotaBusinessTest(mapper, configuration);
            _controller = new QuotaController(_service);

        }
        #endregion 
        [Fact]
        public void CreateQuotas()
        {
            List<QuotaDataDto> quotasMock = new List<QuotaDataDto>();
            for (int i = 1; i <= 5; i++)
            {
                quotasMock.Add(new QuotaDataDto
                {
                    IdQuota = i,
                    IdCredit = 1,
                    CapitalValue = 22500,
                    TotalValue = 23625,
                    PaymentDate = DateTime.Now
                });
            }
            var actionResult = _controller.Create(new CreateQuotasRequest
            {
                Quotas = quotasMock,
                IdClient = 1
            }).Result as ObjectResult;
            var result = actionResult.Value as HttpResponseDto<bool>;
            Assert.NotNull(result);
            Assert.Equal(200, result.Code);
            var response = result.Object;
            Assert.IsType<bool>(response);
            Assert.True(response);
        }
        [Fact]
        public void GetByCredit()
        {
            var actionResult = _controller.GetByIdCredit(1).Result as ObjectResult;
            var result = actionResult.Value as HttpResponseDto<List<QuotaDataDto>>;
            Assert.NotNull(result);
            Assert.Equal(200, result.Code);
            var response = result.Object;
            Assert.NotNull(response);
            Assert.IsType<List<QuotaDataDto>>(response);
            Assert.True(response.Count > 0);
        }
        [Fact]
        public void SimulateQuotas()
        {
            var actionResult = _controller.SimulateQuotas(new SimulateQuotasRequest
            {
                Frequency = 15,
                CapitalValue = 90000,
                TermMonths = 2,
                IdClient = 1
            }).Result as ObjectResult;
            var result = actionResult.Value as HttpResponseDto<SimulateQuotasResponse>;
            Assert.NotNull(result);
            Assert.Equal(200, result.Code);
            var response = result.Object;
            Assert.NotNull(response);
            Assert.IsType<SimulateQuotasResponse>(response);
            Assert.True(response.Quotas.Count > 0);
        }
    }
}
