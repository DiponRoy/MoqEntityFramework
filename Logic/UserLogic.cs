using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Db;
using Db.Context;
using Db.Factory;
using Db.Model;

namespace Logic
{
    public class UserLogic
    {
        public readonly IUmsDbContext DbContext;

        public UserLogic()
        {
            DbContext = new UmsContextFactory().GetDbContext();
        }
        public UserLogic(IUmsDbContext dbContext)
        {
            DbContext = dbContext;
        }


        public User FindByEmail(string email)
        {
            return DbContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public void Add(User user)
        {
            DbContext.Users.Add(user);
            DbContext.SaveChanges();

            var users = DbContext.Users.Where(x => x.Id < 9).ToList();
        }
    }
}
