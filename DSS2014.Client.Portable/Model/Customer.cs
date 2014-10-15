using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2014.Client.Portable.Model
{
    public class Customer
    {
        public Guid Id { get; set; }
        public String Company { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Street { get; set; }
        public String Zip { get; set; }
        public String City { get; set; }
        public String Country { get; set; }
        public String Email { get; set; }
        public String PhotoUrl { get; set; }
    }
}
