using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Moq.EntityFramework.Setups;

namespace Moq.EntityFramework
{
    public class MockDbSet<TDbContext, TSource>
        where TDbContext : class
        where TSource : class 
    {
        public readonly MockDbContext<TDbContext> DbContext;
        public readonly Mock<IDbSet<TSource>> DbSet;

        public MockDbSet(MockDbContext<TDbContext> mockDbContext)
        {
            DbContext = mockDbContext;
            DbSet = new Mock<IDbSet<TSource>>();
        }

        public DbSetSetup<TDbContext, TSource, TResult> SetUp<TResult>(Expression<Func<IDbSet<TSource>, TResult>> expression)
            where TResult : class
        {
            return new DbSetSetup<TDbContext, TSource, TResult>(DbSet.Setup(expression), this);
        }

        public void WithData(List<TSource> list)
        {
            if (list == null)
            {
                throw new NullReferenceException("list is null at WithData");
            }

            var data = list.AsQueryable();
            DbSet.As<IQueryable<TSource>>().Setup(m => m.Provider).Returns(data.Provider);
            DbSet.As<IQueryable<TSource>>().Setup(m => m.Expression).Returns(data.Expression);
            DbSet.As<IQueryable<TSource>>().Setup(m => m.ElementType).Returns(data.ElementType);
            DbSet.As<IQueryable<TSource>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            Type type = typeof(IDbSet<TSource>);
            Type contextType = typeof(TDbContext);
            ParameterExpression parameter = Expression.Parameter(contextType);
            PropertyInfo info = contextType.GetProperties().First(pi => pi.PropertyType == type);
            MemberExpression body = Expression.Property(parameter, info);
            dynamic func = Expression.Lambda(body, parameter);
            DbContext.SetupProperty(func, DbSet.Object);
        }

        public void WithNoData()
        {
            WithData(new List<TSource>());
        }
    }
}
