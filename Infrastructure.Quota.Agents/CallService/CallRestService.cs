using Application.Quota.Dtos;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Quota.Agents.CallService
{
    public static class CallRestService
    {
        /// <summary>
        /// Clase utilizada para realizar consumo de servicios REST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceUrl"></param>
        /// <param name="requestBodyObject"></param>
        /// <param name="verb"></param>
        /// <param name="returnStatus"></param>
        /// <param name="validateCertificate"></param>
        /// <param name="customHeaders"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// 
        public static async Task<object> CallServiceAsync<T>(string serviceUrl, object requestBodyObject, Method verb, string TransactionID, bool returnStatus = false, bool validateCertificate = true,
           Dictionary<string, string> customHeaders = null, Dictionary<string, string> customParams = null, string username = "", string password = "") where T : class
        {

            var client = new RestClient(serviceUrl);
            client.Timeout = -1;
            var request = new RestRequest(verb);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Basic anNpZXJyYUBzaXN0ZWNyZWRpdG8uY29tOmh0V1FZdDJmVThlV3A3Slk=");
            var requestBody = JsonConvert.SerializeObject(requestBodyObject);
            request.AddParameter("application/json", requestBody, ParameterType.RequestBody);
            DateTime salida = DateTime.Now;
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                DateTime llegada = DateTime.Now;
                EscribirArchivo(Diff(salida, llegada), response.Content, TransactionID, response.StatusCode);
                Type typeParameterType = typeof(T);
                try
                {
                    var result = JsonConvert.DeserializeObject<T>(response.Content);
                    return await Task.FromResult(result);
                }
                catch
                {
                    var converter = TypeDescriptor.GetConverter(typeParameterType.GetGenericArguments().FirstOrDefault());
                    var result = converter.ConvertFrom(response.Content);
                    return result;
                }
            }
            else
            {
                DateTime llegada = DateTime.Now;
                EscribirArchivo(Diff(salida, llegada), response.Content, TransactionID, response.StatusCode);
                return await Task.FromResult(default(T));
            }
        }
        private static int Diff(DateTime startDate, DateTime finalDate)
        {
            TimeSpan ts = new TimeSpan();
            ts = finalDate - startDate;
            return ts.Seconds;
        }
        private static void EscribirArchivo(int segundos, string respuesta,
            string id, HttpStatusCode responseCode)
        {

            var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "responseFile.json");
            var jsonResponse = System.IO.File.ReadAllText(filePath);
            FileResponse fileResponse = JsonConvert.DeserializeObject<FileResponse>(jsonResponse);
            if (fileResponse == null) fileResponse = new FileResponse();
            if (fileResponse.ResponseItems == null) fileResponse.ResponseItems = new List<FileResponseItem>();
            var result = JsonConvert.DeserializeObject<IdScanResponse>(respuesta);
            FileResponseItem item = new FileResponseItem
            {
                TransactionID = id,
                Time = segundos,
                ResponseCode = responseCode.ToString(),
                Response = result
            };
            fileResponse.ResponseItems.Add(item);
            int suma = fileResponse.ResponseItems.Sum(c => c.Time);
            fileResponse.Prom = ((double)fileResponse.ResponseItems.Sum(c => c.Time) / (double)fileResponse.ResponseItems.Count);
            System.IO.File.WriteAllText(filePath, JsonConvert.SerializeObject(fileResponse));
        }
    }
}
