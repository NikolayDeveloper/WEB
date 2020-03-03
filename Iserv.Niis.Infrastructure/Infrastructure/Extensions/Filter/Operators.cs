using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Iserv.Niis.Infrastructure.Extensions.Filter
{
    public static class Operators
    {
        public enum Type
        {
            None,
            Sort,
            Order,
            Like,
            GreaterThan,
            GreaterThanOrEqual,
            Equal,
            LessThan,
            LessThanOrEqual,
            FullTextSearch,
            In,
            Contains,
            ContainsDateRange,
            And,
            Or,
            Nand,
            Nor,
            CommonAnd,
            FilterFields
        }

        public const string Delimiter = "_";
        public const char InDelimiter = ',';
        public const string KeyUniqueDiscriminatorPattern = @"^([^\d]+)\d{1,2}$";
        private const string Sort = Delimiter + "sort";
        private const string Order = Delimiter + "order";
        private const string Like = Delimiter + "like";
        private const string GreaterThan = Delimiter + "gt";
        private const string GreaterThanEqual = Delimiter + "gte";
        private const string Equal = Delimiter + "eq";
        private const string LessThan = Delimiter + "lt";
        private const string LessThanEqual = Delimiter + "lte";
        private const string FullTextSearch = Delimiter + "q";
        private const string In = Delimiter + "in";
        private const string Contains = Delimiter + "contains";
        private const string ContainsDateRange = Delimiter + "crange";
        public const string And = "and" + Delimiter;
        private const string Or = "or" + Delimiter;
        private const string Nand = "nand" + Delimiter;
        private const string Nor = "nor" + Delimiter;
        public const string CommonAnd = "cand" + Delimiter;
        public const string FilterFields = Delimiter + "filterfields";

        public static readonly string[] SupportedOperators =
        {
            Sort, Order, Like, GreaterThan, GreaterThanEqual, Equal, LessThan, LessThanEqual, In, FullTextSearch, And,
            Or, Nand, Nor, CommonAnd, Contains, ContainsDateRange, FilterFields
        };

        private static readonly string[] FilterOperators =
        {
            Like, GreaterThan, GreaterThanEqual, Equal, LessThan, LessThanEqual, In, FullTextSearch, Contains,
            ContainsDateRange
        };

        private static readonly Dictionary<Type, Func<Expression, Expression, Expression>> ExpressionMappings =
            new Dictionary<Type, Func<Expression, Expression, Expression>>
            {
                {Type.Like, ToLowerContainsExp},
                {Type.GreaterThan, Expression.GreaterThan},
                {Type.GreaterThanOrEqual, Expression.GreaterThanOrEqual},
                {Type.Equal, Expression.Equal},
                {Type.LessThan, Expression.LessThan},
                {Type.LessThanOrEqual, Expression.LessThanOrEqual},
                {Type.In, Expression.Equal},
                {Type.FullTextSearch, ToLowerContainsExp},
                {Type.Contains, ArrayContainsExp},
                {Type.ContainsDateRange, ArrayContainsExp},
                {Type.And, Expression.AndAlso},
                {Type.Or, Expression.OrElse},
                {Type.Nand, NandExpression},
                {Type.Nor, NorExpression},
                {Type.CommonAnd, Expression.AndAlso}
            };

        public static Func<Expression, Expression, Expression> GetExpressionFunc(Type type)
        {
            return ExpressionMappings[type];
        }

        public static Func<Expression, Expression, Expression> GetExpressionFunc(string code)
        {
            return ExpressionMappings[ToEnum(code)];
        }

        public static bool IsSupported(string code)
        {
            return SupportedOperators.Contains(code);
        }

        public static bool ForFilter(string code)
        {
            return FilterOperators.Contains(code);
        }

        public static Type ToEnum(string code)
        {
            switch (code)
            {
                case Sort:
                    return Type.Sort;
                case Order:
                    return Type.Order;
                case Like:
                    return Type.Like;
                case GreaterThan:
                    return Type.GreaterThan;
                case GreaterThanEqual:
                    return Type.GreaterThanOrEqual;
                case Equal:
                    return Type.Equal;
                case LessThan:
                    return Type.LessThan;
                case LessThanEqual:
                    return Type.LessThanOrEqual;
                case FullTextSearch:
                    return Type.FullTextSearch;
                case In:
                    return Type.In;
                case Contains:
                    return Type.Contains;
                case ContainsDateRange:
                    return Type.ContainsDateRange;
                case And:
                    return Type.And;
                case Or:
                    return Type.Or;
                case Nand:
                    return Type.Nand;
                case Nor:
                    return Type.Nor;
                case CommonAnd:
                    return Type.CommonAnd;
                case FilterFields:
                    return Type.FilterFields;
                default:
                    throw new InvalidEnumArgumentException($"Operator code \"{code}\" does not have match enum type");
            }
        }

        private static Expression ToLowerContainsExp(Expression memberExpression, Expression expression)
        {
            var argumentType = expression.Type;

            if (argumentType == typeof(string))
            {
                var nullCheck = Expression.NotEqual(memberExpression, Expression.Constant(null, typeof(object)));

                //ToLower expression
                var toLowerMethodInfo = typeof(string).GetMethod("ToLower", new System.Type[] { });
                var toLowerCallExp = Expression.Call(memberExpression, toLowerMethodInfo);
                //Contains expression
                var containsMethodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                return Expression.AndAlso(nullCheck, Expression.Call(toLowerCallExp, containsMethodInfo, expression));
            }

            if (argumentType == typeof(int))
            {
                //ToLower expression
                var toStringMethodInfo = typeof(int).GetMethod("ToString", new System.Type[] { });
                var toStringCallExp = Expression.Call(memberExpression, toStringMethodInfo);
                //Contains expression
                var containsMethodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                var value = ((ConstantExpression) expression).Value;
                return Expression.Call(toStringCallExp, containsMethodInfo,
                    Expression.Constant(Convert.ToString(value), typeof(string)));
            }

            if (argumentType == typeof(int?))
            {
                var nullCheck = Expression.NotEqual(memberExpression, Expression.Constant(null, typeof(object)));
                //ToLower expression
                var toStringMethodInfo = typeof(int?).GetMethod("ToString", new System.Type[] { });
                var toStringCallExp = Expression.Call(memberExpression, toStringMethodInfo);
                //Contains expression
                var containsMethodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                var value = ((ConstantExpression)((UnaryExpression)expression).Operand).Value;

                return Expression.AndAlso(nullCheck, Expression.Call(toStringCallExp, containsMethodInfo,
                    Expression.Constant(Convert.ToString(value), typeof(string))));
            }


            throw new ArgumentException(
                $"Contains expression does not support given argument type. Argument type is \"{argumentType}\"");
        }

        private static MethodCallExpression ArrayContainsExp(Expression memberExpression, Expression expression)
        {
            var argumentType = expression.Type;
            var containsMethod = typeof(Enumerable)
                .GetMethods()
                .Where(m => m.Name == "Contains")
                .Single(m => m.GetParameters().Length == 2)
                .MakeGenericMethod(argumentType);
            return Expression.Call(containsMethod, memberExpression, expression);
        }

        private static Expression NandExpression(Expression leftExpression, Expression rightExpression)
        {
            return Expression.Not(Expression.And(leftExpression, rightExpression));
        }

        private static Expression NorExpression(Expression leftExpression, Expression rightExpression)
        {
            return Expression.Not(Expression.Or(leftExpression, rightExpression));
        }
    }
}