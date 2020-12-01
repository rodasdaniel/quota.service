using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net;
using Application.Quota.Common.Handler;
using Application.Quota.Dtos;
using Quota.API.Filters.ValidateModel;
using Application.Quota.Business.Quota;
using System.Collections.Generic;
using Application.Quota.Business.IdScan;

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
    public class IdScanController : ControllerBase
    {
        private readonly IIdScanBusiness _idScanBusiness;
        #region Constructor

        public IdScanController(IIdScanBusiness idScanBusiness)
        {
            _idScanBusiness = idScanBusiness;
        }
        #endregion

        [HttpPost("Scan")]
        [ProducesResponseType(typeof(HttpResponseDto<IdScanResponse>), 200)]
        public async Task<IActionResult> Scan([FromBody] IdScanRequest idScanRequest)
        {
            (HttpStatusCode statusCode, string message, IdScanResponse response) =
                await _idScanBusiness.Scan(idScanRequest);
            if (statusCode != HttpStatusCode.NoContent && Response != null)
            {
                Response.StatusCode = (int)statusCode;
            }
            return ServiceAnswer<IdScanResponse>.Response(statusCode, message, response);
        }
        [HttpPost("MasiveScan")]
        [ProducesResponseType(typeof(HttpResponseDto<bool>), 200)]
        public async Task<IActionResult> MasiveScan()
        {
            (HttpStatusCode statusCode, string message, bool response) =
                await _idScanBusiness.MasiveScan();
            if (statusCode != HttpStatusCode.NoContent && Response != null)
            {
                Response.StatusCode = (int)statusCode;
            }
            return ServiceAnswer<bool>.Response(statusCode, message, response);
        }
    }
}
