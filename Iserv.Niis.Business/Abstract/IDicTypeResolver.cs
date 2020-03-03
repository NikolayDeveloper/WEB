using System;

namespace Iserv.Niis.Business.Abstract
{
    /// <summary>
    /// Интерфейс определения типа справочника по названию
    /// </summary>
    public interface IDicTypeResolver
    {
        /// <summary>
        /// Возвращает тип справочника по названию
        /// </summary>
        /// <param name="dicTypeStr">Название справочника</param>
        /// <returns></returns>
        Type Resolve(string dicTypeStr);
    }
}