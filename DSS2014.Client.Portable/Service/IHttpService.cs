using DSS2014.Client.Portable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2014.Client.Portable.Service
{
    public interface IHttpService
    {
        Task<HttpResponse<String>> ExecuteAsync(string requestUrl, HttpMode mode, string message = null);

        Task<HttpResponse<byte[]>> GetBinaryAsync(string requestUrl);
    }
}
