using TaskManager.Client.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManager.Common.Models;
using Newtonsoft.Json.Linq;
using System.Net;

namespace TaskManager.Client.Services.Tests
{
    [TestClass()]
    public class TasksRequestServiceTests
    {
        [TestMethod()]
        public async Task GetAllTasksTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");

            var service = new TasksRequestService();

            var tasks = await service.GetAllTasks(token);

            Console.WriteLine(tasks.Count);

            Assert.AreNotEqual(Array.Empty<TaskModel>(), tasks.ToArray());
        }

        [TestMethod()]
        public async Task GetTaskByIdTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");

            var service = new TasksRequestService();

            var task = await service.GetTaskById(token, 2);

            Assert.AreNotEqual(null, task);
        }

        [TestMethod()]
        public async Task GetTasksByDeskIdTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");

            var service = new TasksRequestService();

            var tasks = await service.GetTasksByDeskId(token, 1);

            Console.WriteLine(tasks.Count);

            Assert.AreNotEqual(Array.Empty<TaskModel>(), tasks.ToArray());
        }

        [TestMethod()]
        public async Task CreateTaskTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");

            var service = new TasksRequestService();

            var taskModel = new TaskModel("Test", "Test", DateTime.Now, DateTime.Parse("2024-2-16"), "New");
            taskModel.ExecutorId = 1;
            taskModel.DeskId = 6;

            var res = await service.CreateTask(token, taskModel);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task UpdateTaskTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");

            var service = new TasksRequestService();

            var taskModel = new TaskModel("Test2", "Test2", DateTime.Now, DateTime.Parse("2023-11-28"), "New");

            taskModel.DeskId = 1;
            taskModel.Id = 3;

            var res = await service.UpdateTask(token, taskModel);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }

        [TestMethod()]
        public async Task DeleteTaskTest()
        {
            var token = await new UsersRequestService().GetToken("admin", "admin");

            var service = new TasksRequestService();

            var res = await service.DeleteTask(token, 3);

            Assert.AreEqual(HttpStatusCode.OK, res);
        }
    }
}