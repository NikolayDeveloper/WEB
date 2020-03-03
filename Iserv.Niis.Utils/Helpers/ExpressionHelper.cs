using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Iserv.Niis.Utils.Helpers
{
    /// <summary>
    /// Утилитный класс для работ с выражениями.
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// Комбинирует  два выржанеия по заданной логике с одинаковыми входными параметрами.
        /// </summary>
        /// <typeparam name="T">Тип параметра.</typeparam>
        /// <param name="firstExpression">Первое выражение.</param>
        /// <param name="secondExpression">Второе выражени.</param>
        /// <param name="combiner">Логика комбинирования выражения. Смотрите класс <see cref="Expression"/>.</param>
        /// <returns>Скомбинированное выражение.</returns>
        public static Expression<T> CombineExpressions<T>(Expression<T> firstExpression, Expression<T> secondExpression, Func<Expression, Expression, BinaryExpression> combiner)
        {
            var left = firstExpression.Body;
            var right = new ExpressionParameterReplacer(secondExpression.Parameters, firstExpression.Parameters).Visit(secondExpression.Body);

            var combined = Expression.Lambda<T>(combiner(firstExpression.Body, right), firstExpression.Parameters);

            return combined;
        }

        /// <summary>
        /// Скрытый класс для удобного комбинирования выражений одинакого типа.
        /// </summary>
        private class ExpressionParameterReplacer : ExpressionVisitor
        {
            /// <summary>
            /// Словарь подмен параметров выражения.
            /// </summary>
            private readonly IDictionary<ParameterExpression, ParameterExpression> parameterReplacements;

            /// <summary>
            /// Создает новый объект типа <see cref="ExpressionParameterReplacer"/>
            /// </summary>
            /// <param name="fromParameters">Параметры первого выражения.</param>
            /// <param name="toParameters">Параметры вторго выражения.</param>
            public ExpressionParameterReplacer(IList<ParameterExpression> fromParameters, IList<ParameterExpression> toParameters)
            {
                parameterReplacements = new Dictionary<ParameterExpression, ParameterExpression>();

                for (int i = 0; i < fromParameters.Count && i < toParameters.Count; i++)
                {
                    parameterReplacements.Add(fromParameters[i], toParameters[i]);
                }
            }

            /// <summary>
            /// Смотрит, есть ли соответствующая замена параметру.
            /// </summary>
            /// <param name="node">Параметр для подмены.</param>
            /// <returns>Выражение.</returns>
            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (parameterReplacements.TryGetValue(node, out ParameterExpression replacement))
                    node = replacement;
                return base.VisitParameter(node);
            }
        }
    }
}
