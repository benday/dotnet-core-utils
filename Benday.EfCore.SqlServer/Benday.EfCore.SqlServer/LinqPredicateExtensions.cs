using System;
using System.Linq.Expressions;

namespace Benday.EfCore.SqlServer
{
    public static class LinqPredicateExtensions
    {
        public static Expression<Func<T, bool>> And<T>(
            this Expression<Func<T, bool>> value1,
            Expression<Func<T, bool>> value2)
        {
            return Combine(value1, value2, ExpressionType.AndAlso);
        }

        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> value1,
            Expression<Func<T, bool>> value2)
        {
            return Combine(value1, value2, ExpressionType.OrElse);
        }

        private static Expression<Func<T, bool>> Combine<T>(
            this Expression<Func<T, bool>> left,
            Expression<Func<T, bool>> right,
            ExpressionType expressionType)
        {
            ParameterExpression leftParameter0 = left.Parameters[0];

            var visitor = new ParameterSubstitutionVisitor();

            visitor.Substitutions[right.Parameters[0]] = leftParameter0;

            Expression body = Expression.MakeBinary(
                expressionType, left.Body, visitor.Visit(right.Body));

            return Expression.Lambda<Func<T, bool>>(body, leftParameter0);
        }
    }
}
