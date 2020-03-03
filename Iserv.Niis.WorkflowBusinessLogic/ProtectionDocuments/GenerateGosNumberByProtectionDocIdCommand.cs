using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments
{
    public class GenerateGosNumberByProtectionDocIdCommand : BaseCommand
    {
        private readonly INumberGenerator _numberGenerator;

        public GenerateGosNumberByProtectionDocIdCommand(INumberGenerator numberGenerator)
        {
            _numberGenerator = numberGenerator;
        }

        public void Execute(int[] protectionDocumentIds)
        {
            _numberGenerator.GenerateProtectionDocGosNumber(protectionDocumentIds);
        }
    }
}
