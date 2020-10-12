using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net;
using Application.Quota.Common.Handler;
using Application.Quota.Dtos;
using Quota.API.Filters.ValidateModel;
using Application.Quota.Business.Quota;
using System.Collections.Generic;

namespace Quota.API.Controllers
{
    /// <summary>
    /// Controlador encargado de realizar las operaciones de las cuotas
    /// </summary>
    [Route("api/[controller]/v1")]
    [ApiController]
    [ProducesResponseType(typeof(HttpResponseException), 500)]
    [ProducesResponseType(typeof(HttpResponseDto<string>), 400)]
    [ProducesResponseType(typeof(HttpResponseDto<string>), 401)]
    [ProducesResponseType(typeof(HttpResponseDto<string>), 404)]
    [ValidateModel]
    public class QuotaController : ControllerBase
    {
        #region Constructor
        private readonly IQuotaBusiness _quotaBusiness;
        public QuotaController(IQuotaBusiness quotaBusiness)
        {
            _quotaBusiness = quotaBusiness;
        }
        #endregion
        /// <summary>
        /// Método a través del cual se obtienen los datos de las cuotas de un crédito específico.
        /// </summary>
        /// <param name="idCredit">Representa el identificador único del registro de crédito.</param>
        /// <returns></returns>
        [HttpGet("GetByIdCredit/{idCredit}")]
        [ProducesResponseType(typeof(HttpResponseDto<List<QuotaDataDto>>), 200)]
        public async Task<IActionResult> GetByIdCredit(long idCredit)
        {
            (HttpStatusCode statusCode, string message, List<QuotaDataDto> response) =
                await _quotaBusiness.GetByCredit(idCredit);
            if (statusCode != HttpStatusCode.NoContent && Response != null)
            {
                Response.StatusCode = (int)statusCode;
            }
            return ServiceAnswer<List<QuotaDataDto>>.Response(statusCode, message, response);
        }

        /// <summary>
        /// Método a través del cual se realiza una simulación de las cuotas para un crédito.
        /// </summary>
        /// <param name="simulateQuotasRequest">Datos necesarios para la realización de la simulación de las cuotas de un crédito.</param>
        /// <returns></returns>
        [HttpPost("SimulateQuotas")]
        [ProducesResponseType(typeof(HttpResponseDto<SimulateQuotasResponse>), 200)]
        public async Task<IActionResult> SimulateQuotas([FromBody] SimulateQuotasRequest simulateQuotasRequest)
        {
            (HttpStatusCode statusCode, string message, SimulateQuotasResponse response) =
                await _quotaBusiness.SimulateQuotas(simulateQuotasRequest);
            if (statusCode != HttpStatusCode.NoContent && Response != null)
            {
                Response.StatusCode = (int)statusCode;
            }
            return ServiceAnswer<SimulateQuotasResponse>.Response(statusCode, message, response);
        }

        /// <summary>
        /// Método a través del cual se realiza la creación de las cuotas para un crédito.
        /// </summary>
        /// <param name="createQuotasRequest">Datos necesarios para la realización de la creación de las cuotas de un crédito.</param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(HttpResponseDto<bool>), 200)]
        public async Task<IActionResult> Create([FromBody] CreateQuotasRequest createQuotasRequest)
        {
            (HttpStatusCode statusCode, string message, bool response) =
                await _quotaBusiness.CreateQuotas(createQuotasRequest);
            if (statusCode != HttpStatusCode.NoContent && Response != null)
            {
                Response.StatusCode = (int)statusCode;
            }
            return ServiceAnswer<bool>.Response(statusCode, message, response);
        }
    }
}
