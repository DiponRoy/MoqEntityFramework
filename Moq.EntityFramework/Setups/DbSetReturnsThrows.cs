using System;
using System.Data.Entity;
using Moq.Language.Flow;

namespace Moq.EntityFramework.Setups
{
    public class DbSetReturnsThrows<TDbContext, TSource, TResult> where TSource : class where TDbContext : class 
    {
        public readonly IReturnsThrows<IDbSet<TSource>, TResult> Callback;
        public readonly MockDbSet<TDbContext, TSource> MockDbSet;

        public DbSetReturnsThrows(IReturnsThrows<IDbSet<TSource>, TResult> callback, MockDbSet<TDbContext, TSource> mockDbSet)
        {
            Callback = callback;
            MockDbSet = mockDbSet;
        }

        public DbSetReturnsResult<TDbContext, TSource, TResult> Returns<T>(Func<T, TResult> valueFunction)
        {
            Callback.Returns(valueFunction);
            return new DbSetReturnsResult<TDbContext, TSource, TResult>(Callback, MockDbSet);
        }

        public MockDbSet<TDbContext, TSource> Mock()
        {
            return MockDbSet;
        }
    }
}
