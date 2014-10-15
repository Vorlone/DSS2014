using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DSS2014.Client.Portable.Model
{
    public class HttpResponse<T>
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string Message { get; set; }
        public Uri Location { get; set; }

        public bool IsSuccessStatusCode
        {
            get
            {
                if (HttpStatusCode >= HttpStatusCode.OK)
                    return HttpStatusCode <= (HttpStatusCode)299;
                else
                    return false;
            }
        }

        public T Result { get; set; }
    }
}
