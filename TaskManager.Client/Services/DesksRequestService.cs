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
    public class DesksRequestService : CommonRequestService
    {
        private string _desksControllerUrl = HOST + "desks";

        public async Task<List<DeskModel>> GetAllDesks(AuthToken token)
        {
            try
            {
                string responce = await GetDataByUrl(_desksControllerUrl, token);

                if (!string.IsNullOrEmpty(responce))
                {
                    List<DeskModel> desks = JsonConvert.DeserializeObject<List<DeskModel>>(responce);
                    return desks ?? new List<DeskModel>();
                }
                else
                {
                    return new List<DeskModel>();
                }
            }
            catch (Exception)
            {
                return new List<DeskModel>();
            }
        }

        public async Task<DeskModel> GetDeskById(AuthToken token, int deskId)
        {
            var responce = await GetDataByUrl($"{_desksControllerUrl}/{deskId}", token);
            DeskModel desk = JsonConvert.DeserializeObject<DeskModel>(responce);
            return desk ?? new DeskModel();
        }

        public async Task<List<DeskModel>> GetDesksByProjectId(AuthToken token, int projectId)
        {
            var responce = await GetDataByUrl($"{_desksControllerUrl}/project?={projectId}", token);
            List<DeskModel> desk = JsonConvert.DeserializeObject<List<DeskModel>>(responce);
            return desk ?? new List<DeskModel>();
        }

        public async Task<HttpStatusCode> CreateDesk(AuthToken token, DeskModel desk)
        {
            string deskJson = JsonConvert.SerializeObject(desk);
            var result = await SendDataByUrl(HttpMethod.Post, _desksControllerUrl, token, deskJson);
            return result;
        }

        public async Task<HttpStatusCode> UpdateDesk(AuthToken token, DeskModel desk)
        {
            string deskJson = JsonConvert.SerializeObject(desk);
            var result = await SendDataByUrl(HttpMethod.Patch, $"{_desksControllerUrl}/{desk.Id}", token, deskJson);
            return result;
        }

        public async Task<HttpStatusCode> DeleteDesk(AuthToken token, int deskId)
        {
            var result = await DeleteDataByUrl($"{_desksControllerUrl}/{deskId}", token);
            return result;
        }
    }
}
