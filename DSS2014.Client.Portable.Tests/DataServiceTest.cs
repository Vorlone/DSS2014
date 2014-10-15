using DSS2014.Client.Portable.Service;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Threading.Tasks;

namespace DSS2014.Client.Portable.Tests
{
    [TestClass]
    public class DataServiceTest
    {
        [TestMethod]
        public async Task GetCustomers()
        {
            var dataService = new DataService(new HttpService());

            var result = await dataService.GetCustomersAsync();

            Assert.IsNotNull(result.Result);
        }
    }
}
