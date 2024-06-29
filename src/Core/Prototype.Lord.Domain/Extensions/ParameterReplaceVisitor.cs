using System.Linq.Expressions;

namespace Prototype.Lord.Domain.Extensions;

public class ParameterReplaceVisitor : ExpressionVisitor
{
    public ParameterExpression Target { get; set; }
    public ParameterExpression Replacement { get; set; }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == Target ? Replacement : base.VisitParameter(node);
    }
}

public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> True<T>()
    {
        return f => true;
    }

    public static Expression<Func<T, bool>> False<T>()
    {
        return f => false;
    }

    public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> expr1,
                                                        Expression<Func<T, bool>> expr2)
    {
        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<T, bool>>
              (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                Expression<Func<T, bool>> expr2)
    {
        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<T, bool>>
              (Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
    }

    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                         Expression<Func<T, bool>> expr2)
    {
        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
        return Expression.Lambda<Func<T, bool>>
              (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
    }

    public static Expression<Func<T, bool>> AndExpression<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var visitor = new ParameterReplaceVisitor()
        {
            Target = right.Parameters[0],
            Replacement = left.Parameters[0],
        };
        var rewrittenRight = visitor.Visit(right.Body);
        var andExpression = Expression.AndAlso(left.Body, rewrittenRight);
        return Expression.Lambda<Func<T, bool>>(andExpression, left.Parameters);
    }

    public static Expression<Func<T, bool>> OrExpression<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var visitor = new ParameterReplaceVisitor()
        {
            Target = right.Parameters[0],
            Replacement = left.Parameters[0],
        };
        var rewrittenRight = visitor.Visit(right.Body);
        var andExpression = Expression.Or(left.Body, rewrittenRight);
        return Expression.Lambda<Func<T, bool>>(andExpression, left.Parameters);
    }
}