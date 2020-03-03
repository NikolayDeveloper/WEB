using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using Iserv.Niis.Infrastructure.Extensions.Filter;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicCommon
{
    /// <summary>
    /// Запрос для получения справочника с отфлильтрованными данными.
    /// </summary>
    public class GetDictionaryByEntityNameAndFilterByColumnsQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="dictionaryType">Названия справочников, реализующих <see cref="DictionaryEntity{TKey}"/>.</param>
        /// <param name="httpRequest">Представляет входящую сторону отдельного HTTP-запроса.</param>
        /// <returns>Справочник.</returns>
        public async Task<List<dynamic>> ExecuteAsync(DictionaryType dictionaryType, HttpRequest httpRequest)
        {
            var repo = Uow.GetRepository();

            var entityClrType = repo.GetEntityClrType(dictionaryType.ToString());
            dynamic entity = Activator.CreateInstance(entityClrType);

            IQueryable<dynamic> dictionary = repo.AsQueriable(entity);
            var dictionaryClrType = ExecuteCast(entityClrType, dictionary);
            var filteredDictionary = ExecuteFilter(entityClrType, dictionaryClrType, httpRequest.Query);
            
            return await filteredDictionary.Cast<dynamic>().ToListAsync();

            //var repo = Uow.GetRepository(); 

            //var entityClrType = repo.GetEntityClrType(dictionaryType.ToString());
            //dynamic entity = Activator.CreateInstance(entityClrType);

            //IQueryable<dynamic> dictionary = repo.AsQueriable(entity);
            //var dictionaryClrType = ExecuteCast(entityClrType, dictionary);

            //var my = (IQueryable) ExecuteFilter(entityClrType, dictionaryClrType, httpRequest.Query);

            //var result = await my.Cast<dynamic>().ToListAsync();

            //return null;
        }

        /// <summary>
        /// Вызывает метод осуществляющий приведение типов.
        /// </summary>
        /// <param name="genericType">Обобщенный тип метода.</param>
        /// <param name="source">Предоставляет функциональные возможности расчета запросов к конкретному источнику данных, для которого не указан тип данных.</param>
        /// <returns>Результат выполнения метода.</returns>
        private IQueryable ExecuteCast(Type genericType, IQueryable source)
        {
            return (IQueryable) ExecuteStaticGenericMethod(typeof(Queryable),
                new[] {genericType},
                "Cast",
                new object[] {source});
        }

        /// <summary>
        /// Вызывает метод осуществляющий фильтрацию данных.
        /// </summary>
        /// <param name="genericType">Обобщенный тип метода.</param>
        /// <param name="query">Предоставляет функциональные возможности расчета запросов к конкретному источнику данных, для которого не указан тип данных.</param>
        /// <param name="queryCollection">Представляет коллекцию строк запроса HttpRequest.</param>
        /// <returns>Результат выполнения метода.</returns>
        private IQueryable ExecuteFilter(Type genericType, IQueryable query, IQueryCollection queryCollection)
        {
            return (IQueryable) ExecuteStaticGenericMethod(typeof(ResponseCollectionFilterExtensions),
                new[] {genericType},
                "Filter",
                new object[] {query, queryCollection});
        }

        /// <summary>
        /// Выполняет статический обобщенный метод.
        /// </summary>
        /// <param name="classType">Тип класса с методом typeof(Имя класса).</param>
        /// <param name="genericTypes">Обобщенные типы метода.</param>
        /// <param name="method">Статический метод класса.</param>
        /// <param name="parameters">Параметры передаваемые в метод.</param>
        /// <returns>Результат выполнения метода.</returns>
        private object ExecuteStaticGenericMethod(Type classType, Type[] genericTypes, string method, object[] parameters)
        {
            var methodInfo = classType.GetMethod(method, BindingFlags.Static | BindingFlags.Public);

            if (methodInfo == null)
                return null;

            var genericMethodInfo = methodInfo.MakeGenericMethod(genericTypes);
            return genericMethodInfo.Invoke(null, parameters);
        }
    }
}
