using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Db.Context;
using Db.Model;
using FizzWare.NBuilder;
using Logic;
using Moq;
using Moq.EntityFramework.Utilities;
using NUnit.Framework;

namespace Test.Unit
{
    [TestFixture]
    public class UserLogicMoq
    {

        [Test, Explicit]
        public void Add_Failed()
        {
            User aUser = new User()
            {
                Id = 100,
                Name = "The User",
                Email = "User@email.com"
            };
            List<User> users = Builder<User>.CreateListOfSize(5).Build().ToList();
            users.Add(aUser);

            var dbContextMock = new Mock<IUmsDbContext>();
            //dbContextMock.Setup(x => x.Users).Returns(users);   //how to convert users to IDbSet ??
            var logic = new UserLogic(dbContextMock.Object);
            logic.Add(aUser);
        }

        [Test, Explicit]
        public void Add_OneItemAdded_ToDbSet__Failed()
        {
            User aUser = new User()
            {
                Name = "The User",
                Email = "User@email.com"
            };
            List<User> users = Builder<User>.CreateListOfSize(5).Build().ToList();  //already 5 items

            var mockSet = new Mock<DbSet<User>>();
            var data = users.AsQueryable();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var dbContextMock = new Mock<IUmsDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(mockSet.Object);

            var logic = new UserLogic(dbContextMock.Object);
            logic.Add(aUser); //should add another to the dbset
            Assert.AreEqual(6, dbContextMock.Object.Users.Count());
        }

        [Test, Explicit]
        public void Add_AddedItem_HasAll_RequeredFields_Failed()
        {
            User aUser = new User()
            {
                Name = "The User",
                Email = "User@email.com"
            };
            List<User> users = Builder<User>.CreateListOfSize(5).Build().ToList();

            var mockSet = new Mock<DbSet<User>>();
            var data = users.AsQueryable();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var dbContextMock = new Mock<IUmsDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(mockSet.Object);

            var logic = new UserLogic(dbContextMock.Object);
            logic.Add(aUser);

            var addedUser = dbContextMock.Object.Users.Last();
            Assert.AreEqual(aUser.Name, addedUser.Name);
            Assert.AreEqual(aUser.Email, addedUser.Email);
        }


        [Test]
        public void Add_OneItemAdded_ToDbSet__Success()
        {
            User aUser = new User()
            {
                Name = "The User",
                Email = "User@email.com"
            };
            List<User> users = Builder<User>.CreateListOfSize(5).Build().ToList();

            var mockSet = new Mock<DbSet<User>>();
            var data = users.AsQueryable();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(x => x.Add(It.IsAny<User>())).Callback((User userToAdd) => users.Add(userToAdd)); //added a mock callback for add

            var dbContextMock = new Mock<IUmsDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(mockSet.Object);

            var logic = new UserLogic(dbContextMock.Object);
            logic.Add(aUser); //should add another to the dbset
            Assert.AreEqual(6, dbContextMock.Object.Users.Count());
        }

        [Test]
        public void Add_AddedItem_HasAll_RequeredFields_Success()
        {
            User aUser = new User()
            {
                Name = "The User",
                Email = "User@email.com"
            };
            List<User> users = Builder<User>.CreateListOfSize(5).Build().ToList();

            var mockSet = new Mock<DbSet<User>>();
            var data = users.AsQueryable();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(x => x.Add(It.IsAny<User>())).Callback((User userToAdd) => users.Add(userToAdd)); //added a mock callback for add

            var dbContextMock = new Mock<IUmsDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(mockSet.Object);

            var logic = new UserLogic(dbContextMock.Object);
            logic.Add(aUser);

            var addedUser = dbContextMock.Object.Users.Last();
            Assert.AreEqual(aUser.Name, addedUser.Name);
            Assert.AreEqual(aUser.Email, addedUser.Email);
        }


        [Test]
        public void Add()
        {
            User aUser = new User()
            {
                Name = "The User",
                Email = "User@email.com"
            };
            IList<User> users = Builder<User>.CreateListOfSize(5).Build().ToList();

            var data = users.AsQueryable();
            var mockSet = new Mock<DbSet<User>>();
            mockSet.Setup(x => x.Add(It.IsAny<User>())).Callback((User userToAdd) => users.Add(userToAdd)); //changed the position
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var dbContextMock = new Mock<IUmsDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(mockSet.Object);

            var logic = new UserLogic(dbContextMock.Object);
            logic.Add(aUser);

            var addedUser = dbContextMock.Object.Users.Last();
            Assert.AreEqual(aUser.Name, addedUser.Name);
            Assert.AreEqual(aUser.Email, addedUser.Email);
        }

        [Test]
        public void Add_WithHelper()
        {
            User aUser = new User()
            {
                Name = "The User",
                Email = "User@email.com"
            };
            IList<User> users = Builder<User>.CreateListOfSize(5).Build().ToList();

            var dbContextMock = MockedDbContext(users);
            var logic = new UserLogic(dbContextMock.Object);
            logic.Add(aUser);

            var addedUser = dbContextMock.Object.Users.Last();
            Assert.AreEqual(aUser.Name, addedUser.Name);
            Assert.AreEqual(aUser.Email, addedUser.Email);
        }

        private Mock<IUmsDbContext> MockedDbContext(IList<User> users)
        {
            var data = users.AsQueryable();
            var mockSet = new Mock<DbSet<User>>();
            mockSet.Setup(x => x.Add(It.IsAny<User>())).Callback((User userToAdd) => users.Add(userToAdd));
                //changed the position
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var dbContextMock = new Mock<IUmsDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(mockSet.Object);
            return dbContextMock;
        }


        [Test]
        [TestCase("UserEmail@Domain.Com")]
        [TestCase("useremail@domain.com")]
        public void FindByEmail(string email)
        {
            User aUser = new User()
            {
                Id = 100,
                Name = "The User",
                Email = email
            };
            List<User> users = Builder<User>.CreateListOfSize(5).Build().ToList();
            users.Add(aUser);

            var dbContextMock = new Mock<IUmsDbContext>();
            dbContextMock.Setup(x => x.Users).Returns(users.ToDbSet());
            var logic = new UserLogic(dbContextMock.Object);

            User foundUser = logic.FindByEmail(aUser.Email);
            Assert.AreEqual(aUser.Id, foundUser.Id);
            Assert.AreEqual(aUser.Name, foundUser.Name);
            Assert.AreEqual(aUser.Email, foundUser.Email);
        }

    }
}
