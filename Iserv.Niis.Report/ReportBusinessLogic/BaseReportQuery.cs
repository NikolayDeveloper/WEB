using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Report.ReportBusinessLogic
{
    internal abstract class BaseReportQuery : BaseQuery
    {
        internal abstract ReportData Execute(ReportConditionData reportFilterData);
    }
}
