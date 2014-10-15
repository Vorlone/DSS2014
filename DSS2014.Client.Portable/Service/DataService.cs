using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSS2014.Client.Portable.Model;
using Newtonsoft.Json;

namespace DSS2014.Client.Portable.Service
{
    public class DataService : IDataService
    {
        private const string BASE_URL = "https://datevsample.azurewebsites.net/{0}";
        private const string API_URL = "api/{0}";
        private const string ASSET_URL = "Static/{0}";
        private const string CUSTOMER_URL = "Customers/{0}";

        private IHttpService _httpService;

        public DataService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<HttpResponse<IEnumerable<Customer>>> GetCustomersAsync()
        {
            var requestUrl = CreateCustomerUrl();
            var result = await GetDeserializedObject<IEnumerable<Customer>>(requestUrl, HttpMode.Get);

            if (result.IsSuccessStatusCode && result.Result != null)
            {
                foreach (Customer c in result.Result)
                {
                    c.PhotoUrl = ConvertToAbsoluteAssetUrl(c.PhotoUrl);
                }
            }

            return result;
        }

        public async Task<HttpResponse<Customer>> GetCustomerAsync(Guid id)
        {
            var requestUrl = CreateCustomerUrl(id.ToString());
            return await GetDeserializedObject<Customer>(requestUrl, HttpMode.Get);
        }

        public async Task<HttpResponse<Customer>> CreateCustomerAsync(Customer customer)
        {
            var serializedCustomer = JsonConvert.SerializeObject(customer);
            var requestUrl = CreateCustomerUrl();
            return await GetDeserializedObject<Customer>(requestUrl, HttpMode.Post, serializedCustomer);
        }

        public async Task<HttpResponse<Customer>> EditCustomerAsync(Customer customer)
        {
            var serializedCustomer = JsonConvert.SerializeObject(customer);
            var requestUrl = CreateCustomerUrl(customer.Id.ToString());
            return await GetDeserializedObject<Customer>(requestUrl, HttpMode.Put, serializedCustomer);
        }

        public async Task<HttpResponse<Customer>> DeleteCustomerAsync(Guid id)
        {
            var requestUrl = CreateCustomerUrl(id.ToString());
            return await GetDeserializedObject<Customer>(requestUrl, HttpMode.Delete);
        }

        private async Task<HttpResponse<T>> GetDeserializedObject<T>(string requestUrl, HttpMode mode, string message = null)
        {
            var result = await _httpService.ExecuteAsync(requestUrl, mode, message);
            var response = new HttpResponse<T> 
            {
                HttpStatusCode = result.HttpStatusCode,
                Message = result.Message,
                Location = result.Location
            };

            if (result.IsSuccessStatusCode)
            {
                response.Result = await Task.Run(() => { return JsonConvert.DeserializeObject<T>(result.Result); });
            }

            return response;
        }

        private string CreateCustomerUrl(string parameter = "")
        {
            return String.Format(BASE_URL, String.Format(API_URL, String.Format(CUSTOMER_URL, parameter)));
        }

        private string ConvertToAbsoluteAssetUrl(string fileName)
        {
            return String.Format(BASE_URL, String.Format(ASSET_URL, fileName));
        }
    }
}
