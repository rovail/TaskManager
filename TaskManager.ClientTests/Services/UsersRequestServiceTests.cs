using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using TaskManager.Common.Models;
using System.Net;
using Newtonsoft.Json;

namespace TaskManager.Client.Services.Tests
{
    [TestClass()]
    public class UsersRequestServiceTests
    {
        [TestMethod()]
        public async Task GetTokenTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");
            Console.WriteLine(token.access_token);
            Assert.IsNotNull(token);
        }

        [TestMethod()]
        public async Task CreateUserTest()
        {
            var service = new UsersRequestService();

            var token = await service.GetToken("admin", "admin");

            UserModel user = new UserModel("User", "User", "user1", "user1", UserStatus.User, "12345678");

            var result = await service.CreateUser(token, user);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public async Task GetAllUserTest()
        {
            var service = new UsersRequestService();

            var token = await service.GetToken("admin", "admin");

            var result = await service.GetAllUsers(token);

            Console.WriteLine(result.Count);

            Assert.AreNotEqual(Array.Empty<UserModel>(), result.ToArray());
        }

        [TestMethod()]
        public async Task DeleteUserTest()
        {
            var service = new UsersRequestService();

            var token = await service.GetToken("admin", "admin");

            var result = await service.DeleteUser(token, 13);


            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public async Task CreateMultipleUsersTest()
        {
            var service = new UsersRequestService();

            var token = await service.GetToken("admin", "admin");

            UserModel user1 = new UserModel("Mali", "Goos", "mlig@gmail.com", "qwerty", UserStatus.User, "12345678");
            UserModel user2 = new UserModel("Mali2", "Goo2s", "mlig@gmail.com2", "qwerty2", UserStatus.User, "123456782");

            List<UserModel> users = new List<UserModel> { user1, user2 };

            var result = await service.CreateMultipleUsers(token, users);

            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public async Task UpdateUserTest()
        {
            var service = new UsersRequestService();

            var token = await service.GetToken("admin", "admin");

            UserModel user = new UserModel("Mali2222", "Goo2s", "mlig@gmail.com2", "qwerty2", UserStatus.User, "123456782");
            user.Id = 13;

            var result = await service.UpdateUser(token, user);


            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}