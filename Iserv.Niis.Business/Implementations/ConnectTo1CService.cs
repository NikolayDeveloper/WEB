using Iserv.Niis.Business.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Iserv.Niis.Business.Implementations
{
	public class ConnectTo1CService : IConnectTo1CService
	{        
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

		public ConnectTo1CService(IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {            
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
       
        public string GetConnectionString()
        {
            var options = _configuration.GetSection("IntegrationWith1COptions");
            var usr = options.GetSection("Usr").Value;
            var pwd = options.GetSection("Pwd").Value;

            //if (_hostingEnvironment.IsDevelopment())
            //{
            var file = options.GetSection("File").Value;

            return $"File=\"{file}\";Usr=\"{usr}\";Pwd=\"{pwd}\"";
            //}
            //else
            //{
            //    var srvr = options.GetSection("Srvr").Value;
            //    var _ref = options.GetSection("Ref").Value;

            //    return $"Srvr={srvr};Ref={_ref};Usr={usr};Pwd={pwd}";
            //}
        }
    }
}