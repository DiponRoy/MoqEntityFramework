using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Db.Context;
using Db.Model;
using FizzWare.NBuilder;
using Logic;
using Moq;
using Moq.EntityFramework;
using NUnit.Framework;

namespace Test.Unit
{
    [TestFixture]
    public class UserLogicMoqEntityFramework
    {
        [Test]
        [TestCase("UserEmail@Domain.Com")]
        [TestCase("useremail@domain.com")]
        public void FindByEmail(string email)
        {
            var aUser = new User
            {
                Id = 100,
                Name = "The User",
                Email = email
            };
            List<User> users = Builder<User>.CreateListOfSize(5).Build().ToList();
            users.Add(aUser);

            var dbContextMock = new MockDbContext<IUmsDbContext>();
            dbContextMock.SetupDbSet(x => x.Users).WithData(users);
            var logic = new UserLogic(dbContextMock.Object);

            User foundUser = logic.FindByEmail(aUser.Email);
            Assert.IsNotNull(foundUser);
            Assert.AreEqual(100, foundUser.Id);
            Assert.AreEqual(aUser.Name, foundUser.Name);
            Assert.AreEqual(aUser.Email, foundUser.Email);
        }


        [Test]
        public void Add_Count()
        {
            var aUser = new User
            {
                Id = 100,
                Name = "The User",
                Email = "Email"
            };
            List<User> users = Builder<User>.CreateListOfSize(5).Build().ToList();

            var dbContextMock = new MockDbContext<IUmsDbContext>();
            dbContextMock.SetupDbSet(x => x.Users)
                            .SetUp(x => x.Add(It.IsAny<User>()))
                                .Callback((User lol) =>
                                {
                                    ++lol.Id;
                                    users.Add(lol);
                                })
                                .Returns((User lol) => lol)
                                .Mock()
                            .WithData(users);


            var logic = new UserLogic(dbContextMock.Object);
            logic.Add(aUser);


            Assert.AreEqual(6, dbContextMock.Object.Users.Count());
        }

        [Test]
        public void Add_Fields()
        {
            var aUser = new User
            {
                Id = 100,
                Name = "The User",
                Email = "Email"
            };
            List<User> users = Builder<User>.CreateListOfSize(5).Build().ToList();

            var dbContextMock = new MockDbContext<IUmsDbContext>();
            dbContextMock.SetupDbSet(x => x.Users)
                            .SetUp(x => x.Add(It.IsAny<User>()))
                                .Callback((User lol) => users.Add(lol))
                                .Returns((User lol) => lol)
                                .Mock()
                            .WithData(users);

            var logic = new UserLogic(dbContextMock.Object);
            logic.Add(aUser);

            User addedUser = dbContextMock.Object.Users.Last();
            Assert.AreEqual(aUser.Name, addedUser.Name);
            Assert.AreEqual(aUser.Email, addedUser.Email);
        }

        [Test]
        public void Add_Try_With_Extention()
        {
            var aUser = new User
            {
                Id = 100,
                Name = "The User",
                Email = "Email"
            };
            List<User> users = Builder<User>.CreateListOfSize(5).Build().ToList();

            var dbContextMock = new MockDbContext<IUmsDbContext>();
            dbContextMock.SetupDbSet(x => x.Users)
                            .SetUp(x => x.Add(It.IsAny<User>()))
                                .Callback((User lol) => users.Add(lol))
                                .Returns((User lol) => lol)
                                .Mock()
                            .WithData(users);

            var logic = new UserLogic(dbContextMock.Object);
            logic.Add(aUser);

            Assert.AreEqual(6, dbContextMock.Object.Users.Select(x => x).ToList().Count);
        }
    }
}
