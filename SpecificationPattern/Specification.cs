using System.Linq.Expressions;

namespace SpecificationPattern;

public abstract class Specification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();

    public bool IsSatisfiedBy(T candidate)
    {
        Func<T, bool> predicate = ToExpression().Compile(); 

        return predicate(candidate);
    }

    public Specification<T> And(Specification<T> other) => new AndSpecification<T>(this, other);
    public Specification<T> Or(Specification<T> other) => new OrSpecification<T>(this, other);
    public Specification<T> Not() => new NotSpecification<T>(this);

    public static implicit operator Expression<Func<T, bool>>(Specification<T> specification)
    {
        return specification.ToExpression();
    }
}

public class AndSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public AndSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> left = _left.ToExpression();
        Expression<Func<T, bool>> right = _right.ToExpression();

        // One shared parameter for both sides.
        ParameterExpression parameter = Expression.Parameter(typeof(T));

        Expression leftBody = new ReplaceParameterVisitor(left.Parameters[0], parameter).Visit(left.Body);
        Expression rightBody = new ReplaceParameterVisitor(right.Parameters[0], parameter).Visit(right.Body);

        Expression body = Expression.AndAlso(leftBody, rightBody);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}

public class NotSpecification<T> : Specification<T>
{
    private readonly Specification<T> _inner;
    public NotSpecification(Specification<T> inner) => _inner = inner;

    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> expression = _inner.ToExpression();
        Expression body = Expression.Not(expression.Body);
        return Expression.Lambda<Func<T, bool>>(body, expression.Parameters);
    }
}


public class OrSpecification<T> : Specification<T>
{
    private readonly Specification<T> _left;
    private readonly Specification<T> _right;

    public OrSpecification(Specification<T> left, Specification<T> right)
    {
        _left = left;
        _right = right;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> left = _left.ToExpression();
        Expression<Func<T, bool>> right = _right.ToExpression();

        ParameterExpression parameter = Expression.Parameter(typeof(T));

        Expression leftBody = new ReplaceParameterVisitor(left.Parameters[0], parameter).Visit(left.Body);
        Expression rightBody = new ReplaceParameterVisitor(right.Parameters[0], parameter).Visit(right.Body);

        Expression body = Expression.OrElse(leftBody, rightBody);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}

