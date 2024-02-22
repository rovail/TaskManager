using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Common.Models;
using TaskManager.Client.Models;
using System.Net;

namespace TaskManager.Client.Services.Tests
{
    [TestClass()]
    public class ProjectsRequestServiceTests
    {

        [TestMethod()]
        public async Task GetAllProjectsTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");

            var service = new ProjectsRequestService();

            var projects = await service.GetAllProjects(token);

            Console.WriteLine(projects.Count);

            Assert.AreNotEqual(Array.Empty<ProjectModel>(), projects.ToArray());
        }

        [TestMethod()]
        public async Task GetPtojectByIdTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");

            var service = new ProjectsRequestService();

            var project = await service.GetPtojectById(token, 1);

            Assert.AreNotEqual(null, project);
        }

        [TestMethod()]
        public async Task CreateProjectTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");
            var service = new ProjectsRequestService();

            ProjectModel projectModel = new ProjectModel("Test", "Test", ProjectStatus.InProgress, 1);

            var res = await service.CreateProject(token, projectModel);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task UpdateProjectTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");
            var service = new ProjectsRequestService();

            ProjectModel projectModel = new ProjectModel("Test2", "Test2", ProjectStatus.InProgress, 1);
            projectModel.Id = 2;
            var res = await service.UpdateProject(token, projectModel);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task DeleteProjectTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");
            var service = new ProjectsRequestService();

            var res = await service.DeleteProject(token, 1);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task AddUsersToProjectTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");
            var service = new ProjectsRequestService();

            var res = await service.AddUsersToProject(token, 2, new List<int>() { 15, 16, 17 });

            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task RemoveUsersFromProjectTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");
            var service = new ProjectsRequestService();

            var res = await service.RemoveUsersFromProject(token, 2, new List<int>() { 15, 17 });

            Assert.AreEqual(HttpStatusCode.OK, res);
        }
    }
}