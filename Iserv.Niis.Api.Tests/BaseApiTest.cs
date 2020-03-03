using System.Net.Http;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;

namespace Iserv.Niis.Api.Tests
{
    [TestFixture]
    public class BaseApiTest
    {
        private NiisWebContext _apiDbContext;
        private HttpClient _apiHttpClient;

        private NiisWebContext _portalDbContext;
        private HttpClient _portalHttpClient;

        [OneTimeSetUp]
        public void TestMethod1()
        {
            var apiWebHostBuilder = new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>();

            var apiTestServer = new TestServer(apiWebHostBuilder);
            _apiDbContext = apiTestServer.Host.Services.GetService(typeof(NiisWebContext)) as NiisWebContext;
            _apiHttpClient = apiTestServer.CreateClient();

            var portalWebHostBuilder = new WebHostBuilder()
                .UseEnvironment("Testing");
                //.UseStartup<Portal.Startup>()ж

            var portalTestServer = new TestServer(portalWebHostBuilder);
            _portalDbContext = portalTestServer.Host.Services.GetService(typeof(NiisWebContext)) as NiisWebContext;
            _portalHttpClient = portalTestServer.CreateClient();
        }

        protected async Task<(HttpResponseMessage, HttpResponseMessage)> Get(string getActionUrl)
        {
            var apiResponse = await _apiHttpClient.GetAsync(getActionUrl);
            var portalResponse = await _apiHttpClient.GetAsync(getActionUrl);
            return (apiResponse, portalResponse);
        }
    }
}
