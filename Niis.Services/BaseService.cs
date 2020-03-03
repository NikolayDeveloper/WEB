using AutoMapper;
using Iserv.Niis.DI;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Services
{
    public class BaseService
    {
        private IExecutor _executor;
        protected IExecutor Executor => _executor ?? (_executor = NiisAmbientContext.Current.Executor);

        private IMapper _mapper;
        protected IMapper Mapper => _mapper ?? (_mapper = NiisAmbientContext.Current.Mapper);
    }
}