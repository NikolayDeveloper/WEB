using Iserv.Niis.Domain.Enums;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;
using System.Threading.Tasks;

namespace Iserv.Niis.Services.Interfaces
{
    public interface IProtectionDocService
    {
        /// <summary>
        /// Запускает бизнес процесс по ОД.
        /// </summary>
        /// <param name="workFlowRequest">Запрос на бизнес процесс.</param>
        void StartWorkflowProccess(ProtectionDocumentWorkFlowRequest workFlowRequest);

        /// <summary>
        /// Генерирует гос.номер для списка ОД и возвращает массив идентификаторов ОД, для которых сгенерировался гос.номер.
        /// </summary>
        /// <param name="protectionDocIds">Массив идентификаторов охранных документов.</param>
        /// <param name="selectionMode">Тип выбора.</param>
        /// <param name="hasIpc">Есть ли у ОД МПК.</param>
        /// <param name="isAllSelected">Выбраны ли все ОД.</param>
        /// <returns>Массив идентификаторов ОД с сгенерированными гос.номерами.</returns>
        Task<int[]> GenerateGosNumbers(int[] protectionDocIds, SelectionMode selectionMode, bool hasIpc, bool isAllSelected);
        Task CreateAuthorsCertificate(int protectionDocId, int[] authorIds, int userId);
    }
}
