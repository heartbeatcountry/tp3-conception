using System.Linq.Expressions;

namespace CineQuebec.Persistence.Lib;

public static class ExpressionExtensions
{
    public static Expression<Func<TNew, TResult>> ReplaceTypeParameter<TOld, TNew, TResult>(
        this Expression<Func<TOld, TResult>> expression)
    {
        ReplaceTypeVisitor<TOld, TNew> visitor = new();
        Expression newBody = visitor.Visit(expression.Body);
        return Expression.Lambda<Func<TNew, TResult>>(newBody, visitor.NewParameter);
    }

    private class ReplaceTypeVisitor<TOld, TNew> : ExpressionVisitor
    {
        internal readonly ParameterExpression NewParameter = Expression.Parameter(typeof(TNew));

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node.Type == typeof(TOld) ? NewParameter : base.VisitParameter(node);
        }
    }
}