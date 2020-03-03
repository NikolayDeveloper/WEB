using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Iserv.Niis.Business.Exceptions;
using Iserv.Niis.Domain.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Primitives;

namespace Iserv.Niis.Portal.Infrastructure.Extensions.Filter
{
    public static class ResponseCollectionFilterExtensions
    {
        private const string DefaultOrderDirection = nameof(SortOrder.Descending);
        private const string DefaultSortField = nameof(IEntity<int>.Id);
        private const string DefaultCombineOperator = Operators.And;
        private static readonly string[] PaginationQueryKeys = { "_page", "_limit" };

        public static IQueryable<T> Filter<T>(this IQueryable<T> query, IQueryCollection queryCollection)
        {
            var keyValuePairs = queryCollection
                .Where(q => Operators.SupportedOperators.Any(so => q.Key.Contains(so))
                            && !PaginationQueryKeys.Any(p => q.Key.Contains(p)))
                .ToList();

            Validate<T>(keyValuePairs);

            return query
                .InternalFilter<T>(keyValuePairs);
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> query, IQueryCollection queryCollection)
        {
            //var keyValuePairs = queryCollection.Where(q => !PaginationQueryKeys.Any(p => q.Key.Contains(p))).ToList();
            //Validate<T>(keyValuePairs);

            return query
                .Order(queryCollection);
        }

        #region Builders

        private static IQueryable<T> InternalFilter<T>(this IQueryable<T> query,
            IList<KeyValuePair<string, StringValues>> keyValuePairs)
        {
            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var lowPriorityExpressionsTuple =
                    new List<(Func<Expression, Expression, Expression> joinOperator, Expression combinedExpression)>();
                var inExpressions = new List<Expression>();
                Expression combinedHighPriorityExpression = null;
                Expression commonExpression = null;

                var filterValuePairs = keyValuePairs.Where(p => !new[] { "_sort", "_order" }.Contains(p.Key)).ToList();

                foreach (var pair in filterValuePairs)
                {
                    var queryKey = RemoveUniqueDiscriminator(pair.Key);
                    CheckKey(queryKey);

                    var split = Split(queryKey);
                    var operatorCode = split.operatorCode;
                    var joinOperatorCode = split.joinOperatorCode;
                    CheckCode(joinOperatorCode);
                    CheckCode(operatorCode);
                    var extractedValue = Extract(pair.Value);

                    if (AreNotForFiltering<T>(operatorCode, extractedValue)) continue;

                    var expression = BuildExpression<T>(parameter, split.fieldName, operatorCode, extractedValue);
                    var joinOperator = Operators.GetExpressionFunc(joinOperatorCode);

                    if (IsLowPriorityOperator(Operators.ToEnum(joinOperatorCode)) && combinedHighPriorityExpression != null)
                    {
                        lowPriorityExpressionsTuple.Add((joinOperator, combinedHighPriorityExpression));
                        combinedHighPriorityExpression = null;
                    }

                    if (IsCommonOperator(Operators.ToEnum(joinOperatorCode)))
                    {
                        if (commonExpression != null)
                            throw new Exception($"The operator {joinOperatorCode} can only be used once");
                        commonExpression = expression;
                    }
                    else if (IsIncludeJoinWithAndOperatorType<T>(operatorCode, joinOperatorCode))
                    {
                        inExpressions.Add(expression);
                    }
                    else
                    {
                        CombineTo(ref combinedHighPriorityExpression, expression, joinOperator);
                    }

                    if (IsLastPair(filterValuePairs, pair) && combinedHighPriorityExpression != null)
                        lowPriorityExpressionsTuple.Add((null, combinedHighPriorityExpression));
                }

                var combinedExpression = TupleToCombinedExpression(lowPriorityExpressionsTuple) ?? BuildMockExpression();

                return query
                    .Where(Expression.Lambda<Func<T, bool>>(commonExpression ?? BuildMockExpression(), parameter))
                    .Where(Expression.Lambda<Func<T, bool>>(Combine(inExpressions, Expression.AndAlso) ?? BuildMockExpression(), parameter))
                    .Where(Expression.Lambda<Func<T, bool>>(combinedExpression ?? BuildMockExpression(), parameter));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw new FilterException(innerException: e);
            }
        }

        private static Expression TupleToCombinedExpression(
            List<(Func<Expression, Expression, Expression> joinOperator, Expression combinedExpression)>
                lowPriorityExpressions)
        {
            Expression finalExpression = null;
            lowPriorityExpressions.Reverse();

            foreach (var item in lowPriorityExpressions)
                CombineTo(ref finalExpression, item.combinedExpression, item.joinOperator);

            return finalExpression;
        }

        private static void CombineTo(ref Expression sourceExpression, Expression expression,
            Func<Expression, Expression, Expression> joinOperator)
        {
            sourceExpression = sourceExpression == null
                ? expression
                : joinOperator.Invoke(sourceExpression, expression);
        }

        private static Expression BuildExpression<T>(ParameterExpression parameter, string fieldName,
            string operatorCode, string extractedValue)
        {
            var expressionFunc = Operators.GetExpressionFunc(operatorCode);
            
            switch (Operators.ToEnum(operatorCode))
            {
                case Operators.Type.FullTextSearch:
                    return BuildFullTextSearchExpression<T>(parameter, extractedValue, expressionFunc);
                case Operators.Type.In:
                    return BuildIncludeExpression<T>(parameter, fieldName, extractedValue);
                case Operators.Type.Contains:
                    return BuildContainsExpression<T>(parameter, fieldName, extractedValue, expressionFunc);
                case Operators.Type.ContainsDateRange:
                    return BuildContainsRangeExpression<T>(parameter, fieldName, extractedValue, expressionFunc);
                default:
                    var property = GetPropertyInfo<T>(fieldName, operatorCode);
                    return expressionFunc.Invoke(
                        Expression.MakeMemberAccess(parameter, property),
                        BuildNullableConstantExpression<T>(extractedValue, property));
            }
        }

        private static Expression BuildContainsExpression<T>(ParameterExpression parameter, string fieldName,
            string extractedValue, Func<MemberExpression, Expression, Expression> expressionFunc)
        {
            var propertyInfo = GetPropertyInfo<T>(fieldName);

            var expressions = extractedValue.Split(Operators.InDelimiter).Select(value =>
                expressionFunc.Invoke(
                    Expression.MakeMemberAccess(parameter, propertyInfo),
                    BuildNullableConstantExpression<T>(value, propertyInfo))).ToList();

            return Combine(expressions, Expression.OrElse);
        }

        private static Expression BuildContainsRangeExpression<T>(ParameterExpression parameter, string fieldName,
            string extractedValue, Func<MemberExpression, Expression, Expression> expressionFunc)
        {
            var values = extractedValue.Split(Operators.InDelimiter);
            if (values.Length != 2)
            {
                throw new Exception($"ContainsDateRange operator need on two values. Current count is \"{values.Length}\"");
            }

            var property = GetPropertyInfo<T>(fieldName);
            var propertyType = GetPropertyType(property);
            var dateParameter = Expression.Parameter(propertyType, "date");

            var gtoExpression = String.IsNullOrWhiteSpace(values.First())
                ? null
                : Operators.GetExpressionFunc(Operators.Type.GreaterThanOrEqual).Invoke(dateParameter,
                    BuildNullableConstantExpression<T>(values.First(), property));

            var ltExpression = String.IsNullOrWhiteSpace(values.Last())
                ? null
                : Operators.GetExpressionFunc(Operators.Type.LessThan).Invoke(dateParameter,
                    BuildNullableConstantExpression<T>(values.Last(), property));

            var combinedExpression =
                gtoExpression != null && ltExpression != null
                    ? Expression.And(gtoExpression, ltExpression)
                    : gtoExpression ?? ltExpression;

            var anyMethodInfo = typeof(Enumerable).GetMethods().Where(m => m.Name == "Any")
                .Single(mi => mi.GetParameters().Count() == 2);
            var genericAnyMethodInfo = anyMethodInfo.MakeGenericMethod(propertyType);

            // Do not touch namespace inside typeof!
            var toPredicate =
                typeof(Iserv.Niis.Portal.Infrastructure.Extensions.Filter.ResponseCollectionFilterExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                    .Single(m => m.Name.Equals("ToPredicate"));
            var genericToPredicate = toPredicate.MakeGenericMethod(propertyType);

            var predicate = (Expression)genericToPredicate.Invoke(null, new object[] { combinedExpression, dateParameter });

            return Expression.Call(genericAnyMethodInfo, Expression.MakeMemberAccess(parameter, property), predicate);
        }

        private static Expression<Func<T, bool>> ToPredicate<T>(Expression expression, ParameterExpression dateParameter)
        {
            return Expression.Lambda<Func<T, bool>>(
                expression,
                new[] { dateParameter }
            );
        }

        private static Expression BuildIncludeExpression<T>(ParameterExpression parameter, string fieldName,
            string extractedValue)
        {
            var property = GetPropertyInfo<T>(fieldName);
            var listType = typeof(List<>).MakeGenericType(property.PropertyType);
            var list = (IList)Activator.CreateInstance(listType);

            extractedValue.Split(Operators.InDelimiter).ToList().ForEach(splitValue =>
            {
                var value = Convert(splitValue, property);
                list.Add(value);
            });

            var containsMethod = typeof(ICollection<>).MakeGenericType(property.PropertyType).GetMethod("Contains");

            return Expression.Call(Expression.Constant(list), containsMethod,
                Expression.Property(parameter, fieldName));
        }

        private static Expression BuildFullTextSearchExpression<T>(ParameterExpression parameter, string extractedValue,
            Func<MemberExpression, Expression, Expression> expressionFunc)
        {
            var stringProperties = GetStringProperties<T>();
            if (!stringProperties.Any())
                throw new Exception($"Type \"{nameof(T)}\" does not have string properties for full text search");

            var expressions = stringProperties.Select(stringProperty =>
                expressionFunc.Invoke(Expression.MakeMemberAccess(parameter, stringProperty),
                    BuildConstantExpression(extractedValue.ToLower(), stringProperty))).ToList();

            return Combine(expressions, Expression.OrElse);
        }

        private static Expression BuildNullableConstantExpression<T>(string extractedValue, PropertyInfo property)
        {
            var propertyType = GetPropertyType(property);

            return IsNullable(propertyType)
                ? Expression.Convert(BuildConstantExpression(extractedValue.ToLower(), property),
                    propertyType)
                : BuildConstantExpression(extractedValue.ToLower(), property);
        }

        private static Type GetGenericArgument(Type propertyType)
        {
            return propertyType.GetGenericArguments()[0];
        }

        private static Expression BuildConstantExpression(string extractedValue, PropertyInfo property)
        {
            return Expression.Constant(Convert(extractedValue, property));
        }

        private static BinaryExpression BuildMockExpression()
        {
            return Expression.MakeBinary(ExpressionType.And, Expression.Constant(true), Expression.Constant(true));
        }

        #endregion

        #region Predicates

        private static bool AreNotForFiltering<T>(string operatorCode, string extractedValue)
        {
            return !Operators.ForFilter(operatorCode) || string.IsNullOrWhiteSpace(extractedValue);
        }

        private static bool IsIncludeJoinWithAndOperatorType<T>(string operatorCode, string joinOperatorCode)
        {
            return Operators.ToEnum(operatorCode) == Operators.Type.In &&
                   Operators.ToEnum(joinOperatorCode) == Operators.Type.And;
        }

        private static bool IsLastPair(List<KeyValuePair<string, StringValues>> filterValuePairs,
            KeyValuePair<string, StringValues> pair)
        {
            return filterValuePairs.IndexOf(pair) + 1 == filterValuePairs.Count;
        }

        private static bool IsCommonOperator(Operators.Type operatorType)
        {
            return Operators.Type.CommonAnd == operatorType;
        }

        private static bool IsLowPriorityOperator(Operators.Type operatorType)
        {
            return new[] { Operators.Type.Or, Operators.Type.Nor }.Contains(operatorType);
        }

        private static bool IsNullable(Type propertyType)
        {
            return propertyType.IsGenericType &&
                   propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static bool IsEnumerable(Type propertyType)
        {
            return propertyType.IsGenericType &&
                   propertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        #endregion

        #region Helpers

        private static string Extract(StringValues stringValues)
        {
            return stringValues.FirstOrDefault();
        }

        private static object Convert(string extractedValue, PropertyInfo property)
        {
            var typeConverter = TypeDescriptor.GetConverter(GetUnderlyingPropertyType(property));
            return typeConverter.ConvertFrom(extractedValue);
        }

        private static Expression Combine(List<Expression> expressions,
            Func<Expression, Expression, Expression> combineFunc)
        {
            Expression result = null;
            foreach (var expression in expressions)
                result = expressions.IndexOf(expression) == 0
                    ? expression
                    : combineFunc(result, expression);
            return result;
        }

        private static (string joinOperatorCode, string fieldName, string operatorCode) Split(string queryKey)
        {
            CheckKey(queryKey);

            var delimiter = Operators.Delimiter[0];
            var split = queryKey.Split(delimiter);

            switch (split.Length)
            {
                case 3: return ($"{split[0]}{delimiter}", split[1], $"{delimiter}{split[2]}");
                case 2: return ($"{DefaultCombineOperator}", split[0], $"{delimiter}{split[1]}");
                default:
                    throw new Exception(
                        $"Incorrect query key: {queryKey}. Splitted query keys count can not be: \"{split.Length}\"");
            }
        }

        #endregion

        #region Checks

        private static void Validate<T>(IList<KeyValuePair<string, StringValues>> keyValuePairs)
        {
            var errors = new List<string>();
            var commonOperatorCount = 0;
            foreach (var keyValuePair in keyValuePairs)
                try
                {
                    var queryKey = RemoveUniqueDiscriminator(keyValuePair.Key);
                    CheckKey(queryKey);

                    var split = Split(queryKey);
                    CheckCode(split.joinOperatorCode);
                    CheckCode(split.operatorCode);
                    CheckField<T>(split.fieldName, split.operatorCode);

                    commonOperatorCount += split.joinOperatorCode == Operators.CommonAnd ? 1 : 0;
                }
                catch (Exception e)
                {
                    errors.Add(e.Message);
                }

            if (commonOperatorCount > 1)
                errors.Add($"The operator {Operators.CommonAnd} can only be used once");

            if (errors.Any())
            {
                var message = errors.Join();
                Console.WriteLine(message);
                throw new FilterException(message);
            }
        }

        private static string RemoveUniqueDiscriminator(string key)
        {
            var pattern = Operators.KeyUniqueDiscriminatorPattern;
            try
            {
                if (Regex.IsMatch(key, pattern))
                {
                    var withoutDiscriminatorGroupValue = Regex.Matches(key, pattern)
                        .Cast<Match>()
                        .Select(m => m.Groups[1].Value)
                        .Single();

                    return withoutDiscriminatorGroupValue;
                }

                return key;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw new FilterException($"Unique discriminator usage error on key: {key}", e);
            }
        }

        private static void CheckKey(string key)
        {
            var delimiter = Operators.Delimiter[0];
            var split = key.Split(delimiter);

            switch (split.Length)
            {
                case 3:
                case 2: return;
                case 0:
                    throw new Exception($"Incorrect query key: {key}. Delimiter absent. Delimiter is \"{delimiter}\"");
                default:
                    throw new Exception($"Incorrect query key: {key}. Delimiter count \"{delimiter}\" is unsupported");
            }
        }

        private static void CheckCode(string operatorCode)
        {
            if (!Operators.IsSupported(operatorCode))
                throw new Exception($"Unsupported operator code: \"{operatorCode}\"");

            if (!Operators.IsSupported(operatorCode))
                throw new Exception($"Unsupported operator code: \"{operatorCode}\"");

            //if (IsFullTextSearch(operatorCode) && keyValuePairs.Count(p => Operators.ForFilter(Split(p.Key).operatorCode)) != 1)
            //    throw new Exception($"Operator \"{operatorCode}\" can not be used with any other filter operators");
        }

        private static void CheckField<T>(string fieldName, string operatorCode)
        {
            if (string.IsNullOrWhiteSpace(fieldName) && Operators.IsSupported(operatorCode))
            {
                var type = Operators.ToEnum(operatorCode);
                if (type == Operators.Type.FullTextSearch
                    || type == Operators.Type.Sort
                    || type == Operators.Type.Order)
                    return;
            }

            var filteringProperties = GetFilteringProperties<T>(fieldName);

            switch (filteringProperties.Count)
            {
                case 1: return;
                case 0:
                    throw new Exception(
                        $"For query field name: \"{fieldName}\" on type: \"{nameof(T)}\" not found any suitable property");
                default:
                    throw new Exception(
                        $"For query field name: \"{fieldName}\" on type: \"{nameof(T)}\" found more than one suitable properties");
            }
        }

        #endregion

        #region Get methods

        private static Type GetPropertyType(PropertyInfo property)
        {
            var propertyType = property.PropertyType;

            if (IsEnumerable(propertyType))
            {
                propertyType = GetGenericArgument(propertyType);
            }
            return propertyType;
        }

        private static Type GetUnderlyingPropertyType(PropertyInfo property)
        {
            var propertyType = property.PropertyType;

            if (IsEnumerable(propertyType) || IsNullable(propertyType))
            {
                propertyType = GetGenericArgument(propertyType);
            }

            return propertyType;
        }

        private static PropertyInfo GetPropertyInfo<T>(string fieldName, string operatorCode)
        {
            CheckField<T>(fieldName, operatorCode);
            return GetPropertyInfo<T>(fieldName);
        }

        private static PropertyInfo GetPropertyInfo<T>(string fieldName)
        {
            var filteringProperties = GetFilteringProperties<T>(fieldName);
            return filteringProperties.Single();
        }

        private static List<PropertyInfo> GetFilteringProperties<T>(string fieldName)
        {
            return typeof(T).GetProperties().Where(pi => pi.Name.ToLower().Equals(fieldName.ToLower())).ToList();
        }

        private static IList<PropertyInfo> GetStringProperties<T>()
        {
            return typeof(T).GetProperties().Where(x => x.PropertyType == typeof(string)).ToList();
        }

        #endregion

        #region Order

        /// <summary>
        ///     Сортирует исходный запрос с воответствии с условиями
        /// </summary>
        /// <typeparam name="T">Тип по которому закрыто выражение</typeparam>
        /// <param name="query">Исходный запрос</param>
        /// <param name="queryCollection">Словарь параметров запроса для фильтрации</param>
        /// <returns></returns>
        private static IQueryable<T> Order<T>(this IQueryable<T> query, IQueryCollection queryCollection)
        {
            var sortProperty = GetSortFieldNameFromQueryString(query.ElementType, queryCollection);
            if (sortProperty == null) return query;

            var property = query.ElementType.GetProperty(sortProperty,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var parameter = Expression.Parameter(query.ElementType, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var typeArguments = new[] { query.ElementType, property.PropertyType };

            var methodName = GetOrderDirectionFromQueryString(queryCollection) == SortOrder.Ascending
                ? "OrderBy"
                : "OrderByDescending";
            var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, query.Expression,
                Expression.Quote(orderByExp));

            return query.Provider.CreateQuery<T>(resultExp);
        }

        private static string GetSortFieldNameFromQueryString(Type type, IQueryCollection queryCollection)
        {
            queryCollection.TryGetValue("_sort", out var sortValues);
            var sortField = sortValues.Any()
                ? sortValues.First()
                : DefaultSortField;

            return type.GetProperty(sortField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) ==
                   null
                ? DefaultSortField
                : sortField;
        }

        private static SortOrder GetOrderDirectionFromQueryString(IQueryCollection queryCollection)
        {
            var orderString = DefaultOrderDirection;

            if (queryCollection.TryGetValue("_order", out var orderValues))
                orderString = orderValues.FirstOrDefault();
            return orderString.Equals("asc", StringComparison.InvariantCultureIgnoreCase)
                ? SortOrder.Ascending
                : SortOrder.Descending;
        }

        #endregion
    }
}