using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class ChargeRequestPublicDateHandler : BaseHandler
    {
        private readonly ICalendarProvider _calendarProvider;

        public ChargeRequestPublicDateHandler(ICalendarProvider calendarProvider)
        {
            _calendarProvider = calendarProvider;
        }

        public async Task Execute(int requestId)
        {
            var resuest = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(requestId));
            if (resuest == null) return;

            var data = DateTimeOffset.Now;

            if (data.DayOfWeek == DayOfWeek.Friday)
                data = data.AddDays(7);
            else if (data.DayOfWeek < DayOfWeek.Friday)
                data = data.AddDays(5 - (int)data.DayOfWeek);
            else if (data.DayOfWeek > DayOfWeek.Friday)
                data = data.AddDays(7 - (int)data.DayOfWeek).AddDays(5);

            while (true) {
                var resulr = _calendarProvider.IsHoliday(data);
                if (!resulr)
                    break;

                data.AddDays(1);
            }

            resuest.PublishDate = data;
            await Executor.GetCommand<UpdateRequestCommand>().Process(q => q.ExecuteAsync(resuest));
        }
    }
}
