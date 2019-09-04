using System.Collections.Generic;
using System.Linq.Expressions;

namespace Benday.EfCore.SqlServer
{
    public class ParameterSubstitutionVisitor : ExpressionVisitor
    {
        public Dictionary<Expression, Expression> Substitutions = 
            new Dictionary<Expression, Expression>();

        protected override Expression VisitParameter(ParameterExpression expr)
        {
            if (Substitutions.ContainsKey(expr) == false)
            {
                return expr;
            }
            else
            {
                return Substitutions[expr];
            }
        }
    }
}
