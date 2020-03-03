using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries.DicMain
{
    public class DicOnlineRequisitionStatus : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        #region Public codes

        public static class Codes
        {
            /// <summary>
            /// Ожидает оплату
            /// </summary>
            public const string PendingPayment = "CP";

            /// <summary>
            /// Отказ в регистрации
            /// </summary>
            public const string RegistrationRefusal = "N";

            /// <summary>
            /// Ожидает ответ
            /// </summary>
            public const string AwaitingResponse = "CA";

            /// <summary>
            /// отозвано/прекращено
            /// </summary>
            public const string WithdrawnDiscontinued = "DP";

            /// <summary>
            /// Приостановлено
            /// </summary>
            public const string Paused = "DPR";

            /// <summary>
            /// Зарегистрировано
            /// </summary>
            public const string Registered = "F";

            /// <summary>
            /// Отказ в регистрации
            /// </summary>
            public const string RefusalReg = "N";

            /// <summary>
            /// Заявка зарегистрирована
            /// </summary>
            public const string ApplicationRegistered = "R";
        }

        #endregion
    }
}