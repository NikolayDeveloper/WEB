using System;
using System.Collections.Generic;
using System.Text;

namespace Iserv.Niis.Common.Codes
{
    public static class NumberGenerator
    {
        /// <summary>
        /// Код для генерации номера договора
        /// </summary>
        public const string ContractCode = "ContractNum";

        /// <summary>
        /// Код для генерации штрихкода
        /// </summary>
        public const string Barcode = "Barcode";

        /// <summary>
        /// Префикс для кода генерации номера заявки
        /// </summary>
        public const string RequestNumberCodePrefix = "RequestNum_";

        /// <summary>
        /// Код для генерации входящего номера заявки
        /// </summary>
        public const string RequestIncomingNumberCode = "RequestIncomingNumber";

        /// <summary>
        /// Префикс для кода генерации входящего номера документа
        /// </summary>
        public const string DocumentIncomingNumberCodePrefix = "DocumentIncomingNumber_";

        /// <summary>
        /// Префикс для кода генерации входящего номера документа (филиал)
        /// </summary>
        public const string DocumentIncomingNumberFilialCodePrefix = "DocumentIncomingNumberFilial_";

        /// <summary>
        /// Префикс для кода генерации исходящего номера документа (филиал)
        /// </summary>
        public const string DocumentOutgoingNumberFilialCodePrefix = "DocumentOutgoingNumberFilial_";

        /// <summary>
        /// Префикс для кода генерации исходящего номера документа
        /// </summary>
        public const string DocumentOutgoingNumberCodePrefix = "DocumentOutgoingNumber000001_";

        /// <summary>
        /// Код для генерации гос. номера договора
        /// </summary>
        public const string ContractGosNumberCode = "GosNumber";

        /// <summary>
        /// Префикс для генерации гос. номера охранного документа
        /// </summary>
        public const string ProtectionDocNumberCodePrefix = "ProtectionDoc_";

        /// <summary>
        /// Код для генерации гос. номера ОД по МПК
        /// </summary>
        public const string ProtectionDocGosNumberGenerationByIpcCode = "Ipc";

        /// <summary>
        /// Код для генерации гос. номера ОД по номеру заявки
        /// </summary>
        public const string ProtectionDocGosNumberGenerationByRegNumberCode = "RegNumber";

        /// <summary>
        /// Код для генерации номера бюллетеня
        /// </summary>
        public const string BulletinCode = "Bulletin";
    }
}
