using System.Linq;
using System.Linq.Expressions;

namespace Application.Common.Helpers
{
    public class ExpressionHelper : ExpressionVisitor
    {
        public bool HasOrderBy { get; private set; }

        /// <summary>
        /// Overrides the base VisitMethodCall, this is where we can expect the query expression
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && (node.Method.Name == "OrderBy"
                || node.Method.Name == "OrderByDescending"))
                HasOrderBy = true;

            return base.VisitMethodCall(node);
        }
    }
}