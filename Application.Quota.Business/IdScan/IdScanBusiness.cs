using Application.Quota.Dtos;
using Infrastructure.Quota.Agents.IdScan;
using Microsoft.Azure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Application.Quota.Common.Resources.Messages;

namespace Application.Quota.Business.IdScan
{
    public class IdScanBusiness : IIdScanBusiness
    {
        private readonly IIdScanProvider _idScanProvider;
        #region Constructor

        public IdScanBusiness(IIdScanProvider idScanProvider)
        {
            _idScanProvider = idScanProvider;
        }


        #endregion
        public async Task<(HttpStatusCode statusCode, string message, IdScanResponse response)>
            Scan(IdScanRequest idScanRequest)
        {
            IdScanResponse response = await _idScanProvider.Scan(idScanRequest);
            return (HttpStatusCode.OK, SuccessMsg, response);
        }
        public async Task<(HttpStatusCode statusCode, string message, bool response)>
           MasiveScan()
        {
            for (int i = 1; i <= 4; i++) 
            {
                IdScanResponse response = await _idScanProvider.Scan(GetBody(i.ToString()));
            }
            return (HttpStatusCode.OK, SuccessMsg, true);
        }
        private IdScanRequest GetBody(string id)
        {
            return new IdScanRequest
            {
                id = id,
                side1 = new SideDto { contentType = "image/jpg", data = FromAzureToBase64($"front/{id}").Result },
                side2 = new SideDto { contentType = "image/jpg", data = FromAzureToBase64($"post/{id}").Result }
            };
        }
        public async Task<string> FromAzureToBase64(string azureUri)
        {

            Uri blobUri = new Uri($"https://stdrtest.blob.core.windows.net/{azureUri}.jpg");
            CloudBlockBlob blob = new CloudBlockBlob(blobUri);
            await blob.FetchAttributesAsync();//Fetch blob's properties
            byte[] arr = new byte[blob.Properties.Length];
            await blob.DownloadToByteArrayAsync(arr, 0);
            var azureBase64 = Convert.ToBase64String(arr);
            return azureBase64;

            //UriBuilder uriBuilder = new UriBuilder()
            //{
            //    Scheme = "https",
            //    Host = "stdrtest.blob.core.windows.net",
            //    Path = azureUri//post/1.jpg
            //};
            //using (HttpClient client = new HttpClient())
            //{
            //    var resp = await client.GetAsync($"{uriBuilder.ToString()}.jpg");
            //    var content = resp.Content as StreamContent;
            //    var stream = await content.ReadAsStreamAsync();
            //    byte[] arrr = new byte[stream.Length];
            //    var azBase64 = Convert.ToBase64String(arrr);
            //    return azBase64;
            //}
        }
    }
}
