using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Moq.EntityFramework.Utilities
{
    public static class ExtensionsUtility
    {
        public static IDbSet<T> ToDbSet<T>(this IList<T> list) where T : class
        {
            /*http://msdn.microsoft.com/en-us/library/dn314429.aspx#queryTest */
            var data = list.AsQueryable();
            var mockSet = new Mock<IDbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet.Object;
        }

        public static void Data<TDbContext, TSource>(this MockDbSet<TDbContext, TSource> mockDbSet, IList<TSource> list)
            where TDbContext : class
            where TSource : class
        {
            if (list == null)
            {
                throw new NullReferenceException("list is null at WithData");
            }

            var data = list.AsQueryable();
            mockDbSet.DbSet.As<IQueryable<TSource>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.DbSet.As<IQueryable<TSource>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.DbSet.As<IQueryable<TSource>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.DbSet.As<IQueryable<TSource>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            Type type = typeof(IDbSet<TSource>);
            Type contextType = typeof(TDbContext);
            ParameterExpression parameter = Expression.Parameter(contextType);
            PropertyInfo info = contextType.GetProperties().First(pi => pi.PropertyType == type);
            MemberExpression body = Expression.Property(parameter, info);
            dynamic func = Expression.Lambda(body, parameter);
            mockDbSet.DbContext.SetupProperty(func, mockDbSet.DbSet.Object);
        }
    }
}
