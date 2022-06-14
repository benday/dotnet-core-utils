using System.Collections.Generic;
using System.Linq.Expressions;

namespace Benday.EfCore.SqlServer
{
    public class ParameterSubstitutionVisitor : ExpressionVisitor
    {
        public ParameterSubstitutionVisitor()
        {
            Substitutions = new Dictionary<Expression, Expression>();
        }

        public Dictionary<Expression, Expression> Substitutions
        {
            get;
            private set;
        }

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
