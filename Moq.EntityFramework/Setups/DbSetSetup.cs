using System;
using System.Data.Entity;
using Moq.Language.Flow;

namespace Moq.EntityFramework.Setups
{
    public class DbSetSetup<TDbContext, TSource, TResult>
        where TDbContext : class
        where TSource : class 
    {
        public readonly ISetup<IDbSet<TSource>, TResult> SetUp;
        public readonly MockDbSet<TDbContext, TSource> MockDbSet;
        public DbSetSetup(ISetup<IDbSet<TSource>, TResult> setup, MockDbSet<TDbContext, TSource> mockDbSet)
        {
            SetUp = setup;
            MockDbSet = mockDbSet;
        }

        public DbSetReturnsThrows<TDbContext, TSource, TResult> Callback<T>(Action<T> action)
        {
            var returnsThrows = new DbSetReturnsThrows<TDbContext, TSource, TResult>(SetUp.Callback(action), MockDbSet);
            return returnsThrows;
        }
    }
}
