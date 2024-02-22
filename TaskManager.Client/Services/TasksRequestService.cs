using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Client.Models;
using TaskManager.Common.Models;

namespace TaskManager.Client.Services
{
    public class TasksRequestService : CommonRequestService
    {
        private string _tasksControllerUrl = HOST + "tasks";

        public async Task<List<TaskModel>> GetAllTasks(AuthToken token)
        {
            try
            {
                string responce = await GetDataByUrl(_tasksControllerUrl + "/user", token);

                if (!string.IsNullOrEmpty(responce))
                {
                    List<TaskModel> tasks = JsonConvert.DeserializeObject<List<TaskModel>>(responce);
                    return tasks ?? new List<TaskModel>();
                }
                else
                {
                    return new List<TaskModel>();
                }
            }
            catch (Exception)
            {
                return new List<TaskModel>();
            }
        }

        public async Task<TaskModel> GetTaskById(AuthToken token, int deskId)
        {
            var responce = await GetDataByUrl($"{_tasksControllerUrl}/{deskId}", token);
            TaskModel task = JsonConvert.DeserializeObject<TaskModel>(responce);
            return task ?? new TaskModel();
        }

        public async Task<List<TaskModel>> GetTasksByDeskId(AuthToken token, int deskId)
        {
            var responce = await GetDataByUrl($"{_tasksControllerUrl}/?deskId={deskId}", token);
            List<TaskModel> tasks = JsonConvert.DeserializeObject<List<TaskModel>>(responce);
            return tasks ?? new List<TaskModel>();
        }

        public async Task<HttpStatusCode> CreateTask(AuthToken token, TaskModel Task)
        {
            string TaskJson = JsonConvert.SerializeObject(Task);
            var result = await SendDataByUrl(HttpMethod.Post, _tasksControllerUrl, token, TaskJson);
            return result;
        }

        public async Task<HttpStatusCode> UpdateTask(AuthToken token, TaskModel Task)
        {
            string TaskJson = JsonConvert.SerializeObject(Task);
            var result = await SendDataByUrl(HttpMethod.Patch, $"{_tasksControllerUrl}/{Task.Id}", token, TaskJson);
            return result;
        }

        public async Task<HttpStatusCode> DeleteTask(AuthToken token, int TaskId)
        {
            var result = await DeleteDataByUrl($"{_tasksControllerUrl}/{TaskId}", token);
            return result;
        }
    }
}
