using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
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
        public static async Task<object> CallServiceAsync<T>(string serviceUrl, object requestBodyObject, Method verb, bool returnStatus = false, bool validateCertificate = true,
           Dictionary<string, string> customHeaders = null, Dictionary<string, string> customParams = null, string username = "", string password = "") where T : class
        {
            try
            {
                var client = new RestClient(serviceUrl);
                var request = new RestRequest(verb);
                client.FollowRedirects = true;
                client.MaxRedirects = 2;
                if (!validateCertificate)
                {
                    ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                    client.RemoteCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                }
                if (requestBodyObject != null)
                {
                    var requestBody = JsonConvert.SerializeObject(requestBodyObject);
                    request.AddHeader("Content-Length", requestBody.Length.ToString());
                    request.AddParameter("", requestBody, ParameterType.RequestBody);
                }
                request.UseDefaultCredentials = true;
                if (customHeaders != null)
                {
                    foreach (KeyValuePair<string, string> header in customHeaders)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }
                if (customParams != null)
                {
                    foreach (KeyValuePair<string, string> param in customParams)
                    {
                        request.AddParameter(param.Key, param.Value);
                    }
                }
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Connection", "keep-alive");
                request.AddHeader("Accept-Encoding", "gzip, deflate");
                request.AddHeader("Accept", "*/*");
                if (customParams == null) request.AddHeader("Content-Type", "application/json");
                else request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                IRestResponse response = client.Execute(request);
                if (returnStatus)
                {
                    return response;
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
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
                    return await Task.FromResult(default(T));
                }
            }
            catch
            {
                return default(T);
            }
        }
    }
}
