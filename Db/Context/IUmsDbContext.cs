using System;
using System.Data.Entity;
using Db.Model;

namespace Db.Context
{
    public interface IUmsDbContext : IDisposable
    {
        IDbSet<User> Users { get; set; }

        int SaveChanges();
    }
}
