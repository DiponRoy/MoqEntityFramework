using System;
using System.Data.Entity;

namespace Moq.EntityFramework
{
    public class MockDbContext<TDbContext> : Mock<TDbContext> where TDbContext : class 
    {
        public MockDbContext()
        {
            
        }
        public MockDbSet<TDbContext, TDbSet> SetupDbSet<TDbSet>(Func<TDbContext, IDbSet<TDbSet>> func) where TDbSet : class 
        {
            return new MockDbSet<TDbContext, TDbSet>(this);
        }
    }
}
