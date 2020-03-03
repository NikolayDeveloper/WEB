using System.Threading.Tasks;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.Document;

namespace Iserv.Niis.Business.Abstract
{
    /// <summary>
    /// Генератор номеров
    /// </summary>
    public interface INumberGenerator
    {
        /// <summary>
        /// Генерирует штрихкод для сущностей со штрихкодами
        /// </summary>
        /// <param name="haveBarcode"></param>
        void GenerateBarcode(IHaveBarcode haveBarcode);

        /// <summary>
        /// Генерирует регистрационный номер для заявки
        /// </summary>
        /// <param name="request"> Заявка, для которого генерируется номер</param>
        /// <returns>Номер заявки</returns>
        Task GenerateRequestNum(Request request);

        /// <summary>
        /// Генерирует входящий номер для заявки
        /// </summary>
        /// <param name="request">Заявка, для которого генерируется номер</param>
        void GenerateIncomingNum(Request request);

        void GenerateIncomingNum(Document document);

        void GenerateOutgoingNum(Document document);

        void GenerateNumForRegisters(Document document);

        /// <summary>
        /// Генерирует следующий номер для код-дискриминатора
        /// </summary>
        /// <param name="code">Код дискриминатор (разделитель)</param>
        /// <returns>Сгенерированный следующий номер</returns>
        int Generate(string code);

        /// <summary>
        /// Генерирует следующий номер государственной регистрации договора
        /// </summary>
        /// <param name="contract"></param>
        /// Шаблон:	[XX] [ГГГГ] [YYY] / [ZZ] - [AA]
        /// [XX] - Код из справочника «Вид ОПС»
        /// [ГГГГ] - Текущий год(год регистрации)
        /// [YYY] - Порядковый номер
        /// [ZZ] - Код из справочника «Виды договора»
        /// [AA] - Код из справочника «Категория договора»
        void GenerateGosNumber(Contract contract);

        /// <summary>
        /// Генерация гос. номеров ОД
        /// </summary>
        /// <param name="ids">Идентификаторы ОД</param>
        void GenerateProtectionDocGosNumber(int[] ids);

        /// <summary>
        /// Генерация входящего номера для договора
        /// </summary>
        /// <param name="contract"></param>
        void GenerateIncomingNum(Contract contract);
    }
}