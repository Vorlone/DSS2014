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

            switch (mode)
            {
                case HttpMode.Get:
                    result = await _httpClient.GetAsync(requestUrl);
                    break;
                case HttpMode.Post:
                    result = await _httpClient.PostAsync(requestUrl, requestContent);
                    break;
                case HttpMode.Put:
                    result = await _httpClient.PutAsync(requestUrl, requestContent);
                    break;
                case HttpMode.Delete:
                    result = await _httpClient.DeleteAsync(requestUrl);
                    break;
                default:
                    break;
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
    }
}
