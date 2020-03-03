using Iserv.Niis.Domain.Enums;

namespace NetCoreWorkflow.Model
{
    internal class CreateDocumentOnStageRule
    {
        internal string DocumentCode { get; set; }
        internal string StageCode { get; set; }
        internal DocumentType DocumentType { get; set; }
    }
}
