using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Db.Context;

namespace Db.Factory
{
    public class UmsContextFactory
    {
        private static IUmsDbContext _dbContext;

        public IUmsDbContext GetDbContext()
        {
            return _dbContext ?? new UmsDbContext();
        }

        public static void SetDbContext(IUmsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
