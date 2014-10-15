using DSS2014.Client.Portable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DSS2014.Client.Portable.Service
{
    public class HttpService : IHttpService
    {
        private HttpClient _httpClient = new HttpClient();

        public async Task<HttpResponse<String>> ExecuteAsync(string requestUrl, HttpMode mode, string message = null)
        {
            HttpContent requestContent = null;
            HttpResponseMessage result = null;
            if (!String.IsNullOrEmpty(message))
                requestContent = new StringContent(message, Encoding.UTF8, "application/json");

            try
            {
                result = await ExecuteHttpAsync(requestUrl, mode, requestContent);
            }
            catch (Exception ex)
            {
                return new HttpResponse<string>
                {
                    HttpStatusCode = System.Net.HttpStatusCode.ServiceUnavailable,
                    Message = ex.Message
               };
            }

            var content = await result.Content.ReadAsStringAsync();

            return new HttpResponse<string> 
            {
                HttpStatusCode = result.StatusCode,
                Message = content,
                Location = result.RequestMessage.RequestUri,
                Result = content
            };
        }

        private async Task<HttpResponseMessage> ExecuteHttpAsync(string requestUrl, HttpMode mode, HttpContent requestContent)
        {
            switch (mode)
            {
                case HttpMode.Get:
                    return await _httpClient.GetAsync(requestUrl);
                case HttpMode.Post:
                    return await _httpClient.PostAsync(requestUrl, requestContent);
                case HttpMode.Put:
                    return await _httpClient.PutAsync(requestUrl, requestContent);
                case HttpMode.Delete:
                    return await _httpClient.DeleteAsync(requestUrl);
                default:
                    return null;
            }
        }
    }
}
