using System.Data.Entity;
using Moq.Language.Flow;

namespace Moq.EntityFramework.Setups
{
    public class DbSetReturnsResult<TDbContext, TSource, TResult> 
        where TDbContext : class 
        where TSource : class 
    {
        public readonly IReturnsThrows<IDbSet<TSource>, TResult> Callback;
        public readonly MockDbSet<TDbContext, TSource> MockDbSet;

        public DbSetReturnsResult(IReturnsThrows<IDbSet<TSource>, TResult> callback, MockDbSet<TDbContext, TSource> mockDbSet)
        {
            Callback = callback;
            MockDbSet = mockDbSet;
        }

        public MockDbSet<TDbContext, TSource> Mock()
        {
            return MockDbSet;
        }
    }
}
