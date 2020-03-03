using System;

namespace Iserv.Niis.Domain.Helpers
{
    public static class Owner
    {
        /// <summary>
        ///     Типы родительских сущностей
        /// </summary>
        public enum Type
        {
            None,

            /// <summary>
            ///     Заявка
            /// </summary>
            Request,

            /// <summary>
            ///     Охранный документ
            /// </summary>
            ProtectionDoc,

            /// <summary>
            ///     Договор коммерциализации
            /// </summary>
            Contract,
            
            /// <summary>
            ///     Материал
            /// </summary>
            Material
        }

        /// <summary>
        ///     Преобразовать тип типа сущности владельца в string
        /// </summary>
        /// <param name="type">Тип сущности обладателя</param>
        /// <returns>Строковое представление</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string ToString(Type type)
        {
            switch (type)
            {
                case Type.None:
                    return "Неизвестен тип сущности обладателя";
                case Type.Request:
                    return "Заявка";
                case Type.ProtectionDoc:
                    return "Охранный документ";
                case Type.Contract:
                    return "Договор";
                case Type.Material:
                    return "Материал";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return "";
        }
    }
}