using DSS2014.Client.Portable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2014.Client.Portable.Service
{
    public interface IDataService
    {
        Task<HttpResponse<IEnumerable<Customer>>> GetCustomersAsync();

        Task<HttpResponse<Customer>> GetCustomerAsync(Guid id);

        Task<HttpResponse<Customer>> CreateCustomerAsync(Customer customer);

        Task<HttpResponse<Customer>> EditCustomerAsync(Customer customer);

        Task<HttpResponse<Customer>> DeleteCustomerAsync(Guid id);

        Task<HttpResponse<byte[]>> GetPhotoAsync(string fileName);
    }
}
