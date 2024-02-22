using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Client.Models;
using TaskManager.Common.Models;
using DryIoc;
using System.Diagnostics;

namespace TaskManager.Client.Services
{
    public class UsersRequestService : CommonRequestService
    {
        private  string _usersControllerUrl = HOST + "users";
        

        public async Task<AuthToken?> GetToken(string username, string password)
        {
            string url = HOST + "account/token";
            string resultStr = await GetDataByUrl(url, null, username, password);

            try
            {
                AuthToken token = JsonConvert.DeserializeObject<AuthToken>(resultStr);
                return token ?? null;
            }
            catch (JsonException)
            {
                return null;
            }
        }

        public async Task<HttpStatusCode> CreateUser(AuthToken token, UserModel user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            var result = await SendDataByUrl(HttpMethod.Post, _usersControllerUrl + "/register", token, userJson);
            return result;
        }

        public async Task<List<UserModel>> GetAllUsers(AuthToken token)
        {
            try
            {
                string responce = await GetDataByUrl(_usersControllerUrl, token);

                if (!string.IsNullOrEmpty(responce))
                {
                    List<UserModel> users = JsonConvert.DeserializeObject<List<UserModel>>(responce);
                    return users ?? new List<UserModel>();
                }
                else
                {
                    return new List<UserModel>();
                }
            }
            catch (Exception)
            {
                return new List<UserModel>();
            }
        }

        public async Task<UserModel?> GetCurrentUser(AuthToken token)
        {
            try
            {
                string responce = await GetDataByUrl($"{HOST}account/info ", token);

                if (!string.IsNullOrEmpty(responce))
                {
                    UserModel user = JsonConvert.DeserializeObject<UserModel>(responce);
                    return user ?? null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<UserModel?> GetUserById(AuthToken token, int userId)
        {
            try
            {
                string responce = await GetDataByUrl(_usersControllerUrl + $"/{userId}", token);

                if (!string.IsNullOrEmpty(responce))
                {
                    UserModel user = JsonConvert.DeserializeObject<UserModel>(responce);
                    return user ?? null;
                }
                else
                {
                    return null;
                    ;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<HttpStatusCode> DeleteUser(AuthToken token, int userId)
        {
            var result = await DeleteDataByUrl($"{_usersControllerUrl}/{userId}", token);
            return result;
        }

        public async Task<HttpStatusCode> CreateMultipleUsers(AuthToken token, List<UserModel> users)
        {
            string userJson = JsonConvert.SerializeObject(users);
            var result = await SendDataByUrl(HttpMethod.Post, _usersControllerUrl + "/all", token, userJson);
            return result;
        }

        public async Task<HttpStatusCode> UpdateUser(AuthToken token, UserModel user)
        {
            try
            {
                string userJson = JsonConvert.SerializeObject(user);
                var result = await SendDataByUrl(HttpMethod.Patch, $"{_usersControllerUrl}/{user.Id}", token, userJson);
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
