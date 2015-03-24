using System.Data.Entity;
using Db.Model;

namespace Db.Context
{
    public class UmsDbContext : DbContext, IUmsDbContext
    {
        public IDbSet<User> Users { get; set; }
    }
}
