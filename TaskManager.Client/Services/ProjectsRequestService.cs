using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Client.Models;
using TaskManager.Common.Models;

namespace TaskManager.Client.Services
{
    public class ProjectsRequestService : CommonRequestService
    {
        private string _projectsControllerUrl = HOST + "projects";
        public async Task<List<ProjectModel>> GetAllProjects(AuthToken token)
        {
            try
            {
                string responce = await GetDataByUrl(_projectsControllerUrl, token);

                if (!string.IsNullOrEmpty(responce))
                {
                    List<ProjectModel> projects = JsonConvert.DeserializeObject<List<ProjectModel>>(responce);
                    return projects ?? new List<ProjectModel>();
                }
                else
                {
                    return new List<ProjectModel>();
                }
            }
            catch (Exception)
            {
                return new List<ProjectModel>();
            }
        }
        public async Task<ProjectModel> GetPtojectById(AuthToken token, int projectId)
        {
            var responce = await GetDataByUrl($"{_projectsControllerUrl}/{projectId}", token);
            ProjectModel project = JsonConvert.DeserializeObject<ProjectModel>(responce);
            return project ?? new ProjectModel();
        }
        public async Task<HttpStatusCode> CreateProject(AuthToken token, ProjectModel project)
        {
            string projectJson = JsonConvert.SerializeObject(project);
            var result = await SendDataByUrl(HttpMethod.Post, _projectsControllerUrl, token, projectJson);
            return result;
        }
        public async Task<HttpStatusCode> UpdateProject(AuthToken token, ProjectModel project)
        {
            try
            {
                string projectJson = JsonConvert.SerializeObject(project);
                var result = await SendDataByUrl(HttpMethod.Patch, $"{_projectsControllerUrl}/{project.Id}", token, projectJson);
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in UpdateUser: {ex.Message}");
                return HttpStatusCode.InternalServerError;
            }
        }
        public async Task<HttpStatusCode> DeleteProject(AuthToken token, int projectId)
        {
            var result = await DeleteDataByUrl($"{_projectsControllerUrl}/{projectId}", token);
            return result;
        }
        public async Task<HttpStatusCode> AddUsersToProject(AuthToken token, int projectId, List<int> usersId)
        {
            try
            {
                string usersIdJson = JsonConvert.SerializeObject(usersId);
                var result = await SendDataByUrl(HttpMethod.Patch, $"{_projectsControllerUrl}/{projectId}/users", token, usersIdJson);
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in UpdateUser: {ex.Message}");
                return HttpStatusCode.InternalServerError;
            }
        }
        public async Task<HttpStatusCode> RemoveUsersFromProject(AuthToken token, int projectId, List<int> usersId)
        {
            try
            {
                string usersIdJson = JsonConvert.SerializeObject(usersId);
                var result = await SendDataByUrl(HttpMethod.Patch, $"{_projectsControllerUrl}/{projectId}/users/remove", token, usersIdJson);
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in UpdateUser: {ex.Message}");
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
