using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using TaskManager.Common.Models;

namespace TaskManager.Client.Services.Tests
{
    [TestClass()]
    public class DesksRequestServiceTests
    {
        [TestMethod()]
        public async Task GetAllDesksTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");

            var service = new DesksRequestService();

            var desks = await service.GetAllDesks(token);

            Console.WriteLine(desks.Count);

            Assert.AreNotEqual(Array.Empty<DeskModel>(), desks.ToArray());
        }

        [TestMethod()]
        public async Task GetDeskByIdTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");

            var service = new DesksRequestService();

            var desks = await service.GetDeskById(token, 1);


            Assert.AreNotEqual(null, desks);
        }

        [TestMethod()]
        public async Task GetDeskByProjectIdTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");

            var service = new DesksRequestService();

            var desks = await service.GetDesksByProjectId(token, 4);


            Assert.AreNotEqual(null, desks);
        }

        [TestMethod()]
        public async Task CreateDeskTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");

            var service = new DesksRequestService();

            var deskModel = new DeskModel("Test", "Test", true, new string[] { "new", "comlite" });
            deskModel.AdminId = 1;
            deskModel.ProjectId = 2;

            var res = await service.CreateDesk(token, deskModel);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task UpdateDeskTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");

            var service = new DesksRequestService();

            var deskModel = new DeskModel("Test2", "Test2", true, new string[] { "new", "comlite" });
            deskModel.AdminId = 1;
            deskModel.ProjectId = 4;
            deskModel.Id = 4;

            var res = await service.UpdateDesk(token, deskModel);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task DeleteDeskTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");

            var service = new DesksRequestService();

            var res = await service.DeleteDesk(token, 3);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }
    }
}