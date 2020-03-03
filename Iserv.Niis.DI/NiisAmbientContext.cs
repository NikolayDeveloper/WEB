using System;
using AutoMapper;
using Iserv.Niis.Authentication;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataBridge.Repositories;
using Iserv.Niis.DI.DateTimeProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.DI
{
    public class NiisAmbientContext
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IConfiguration _configuration;

        private static NiisAmbientContext _current;

        public static NiisAmbientContext Current
        {
            get
            {
                if (_current == null)
                {
                    throw new Exception($"{nameof(NiisAmbientContext)} current is null");
                }

                return _current;
            }
        }

        public NiisAmbientContext(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;

            _current = this;
        }

        public IConfiguration Configuration => _configuration;

        public INiisUserAuthenticationService User => _serviceProvider.GetRequiredService<INiisUserAuthenticationService>();

        public IDateTimeProvider DateTimeProvider => _serviceProvider.GetRequiredService<IDateTimeProvider>();

        public ICalendarProvider CalendarProvider => _serviceProvider.GetRequiredService<ICalendarProvider>();

        // public IExecutor Executor => _serviceProvider.GetRequiredService<IExecutor>();
         public IExecutor Executor => _serviceProvider.GetRequiredService<IExecutor>();

        public IMapper Mapper => _serviceProvider.GetRequiredService<IMapper>();
    }
}