using System.Linq.Expressions;

namespace OnlineMarket.Application.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<Func<IQueryable<T>, IQueryable<T>>> ThenIncludeExpressions { get; } = new List<Func<IQueryable<T>, IQueryable<T>>>();

        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected void AddThenInclude(Func<IQueryable<T>, IQueryable<T>> thenIncludeExpression)
        {
            ThenIncludeExpressions.Add(thenIncludeExpression);
        }
    }
}
