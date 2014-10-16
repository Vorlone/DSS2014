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
        private const string CUSTOMERS_CACHE_KEY = "customers.json";

        private IHttpService _httpService;
        private ILocalStorageService _localStorageService;

        public DataService(IHttpService httpService, ILocalStorageService localStorageService)
        {
            _httpService = httpService;
            _localStorageService = localStorageService;
        }

        public async Task<HttpResponse<IEnumerable<Customer>>> GetCustomersAsync()
        {
            var requestUrl = CreateCustomerUrl();
            return await GetDeserializedObject<IEnumerable<Customer>>(requestUrl, HttpMode.Get, null, true, CUSTOMERS_CACHE_KEY);
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

        public async Task<HttpResponse<byte[]>> GetPhotoAsync(string fileName)
        {
            var requestUrl = ConvertToAbsoluteAssetUrl(fileName);
            var result = await _httpService.GetBinaryAsync(requestUrl);
            if (result.IsSuccessStatusCode)
            {
                await _localStorageService.WriteBinaryToStorageAsync(fileName, result.Result);
            }
            else
            {
                var cachedPhoto = await _localStorageService.ReadBinaryFromStorageAsync(fileName);
                if (cachedPhoto != null)
                {
                    result.Result = cachedPhoto;
                }
            }

            return result;
        }

        private async Task<HttpResponse<T>> GetDeserializedObject<T>(string requestUrl, HttpMode mode, string message = null, bool useCache = false, string cacheKey = null)
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
                if (useCache)
                    await _localStorageService.WriteStringToStorageAsync(cacheKey, result.Result);
            }
            else
            {
                if (useCache)
                {
                    var cachedJsonString = await _localStorageService.ReadStringFromStorageAsync(cacheKey);
                    if (!String.IsNullOrEmpty(cachedJsonString))
                        response.Result = await Task.Run(() => { return JsonConvert.DeserializeObject<T>(cachedJsonString); });
                }
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
