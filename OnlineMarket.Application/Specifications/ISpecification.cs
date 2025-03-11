using System.Linq.Expressions;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }

    List<Expression<Func<T, object>>> Includes { get; }

    List<Func<IQueryable<T>, IQueryable<T>>> ThenIncludeExpressions { get; }
}
