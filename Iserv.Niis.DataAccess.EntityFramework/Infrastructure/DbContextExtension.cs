using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Iserv.Niis.DataAccess.EntityFramework.Infrastructure
{
    /// <summary>
    /// Методы расширения для контекста Entity Framework
    /// </summary>
    public static class DbContextExtension
    {
        /// <summary>
        /// Перегрузка метода Set<T>. Принимает экземпляр Type сущности
        /// </summary>
        /// <param name="context">Контекст Entity Framework</param>
        /// <param name="type">Тип сущности</param>
        /// <returns></returns>
        public static IQueryable Set(this DbContext context, Type type)
        {
            if (type == null)
                throw new ArgumentNullException();

            var typeOfContext = context.GetType();
            var method = typeOfContext.GetMethod("Set");
            var genericMethod = method.MakeGenericMethod(type);
            var queryable = genericMethod.Invoke(context, null) as IQueryable;

            return queryable;
        }

        /// <summary>
        /// Возвращает наименование таблицы
        /// </summary>
        /// <param name="context">Контекст Entity Framework</param>
        /// <param name="type">Тип сущности</param>
        /// <returns></returns>
        public static string GetTableNameByEntityType(this DbContext context, Type type)
        {
            var mapping = context.Model.FindEntityType(type).Relational();
            var tableName = mapping.TableName;
            return tableName;
        }
    }
}