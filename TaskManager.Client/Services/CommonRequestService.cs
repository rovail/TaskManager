using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Client.Models;

namespace TaskManager.Client.Services
{
    public abstract class CommonRequestService
    {
        public const string HOST = "http://taskmanagerapi-dev.eba-etmzbemf.eu-central-1.elasticbeanstalk.com/api/";

        protected async Task<string> GetDataByUrl(string url, AuthToken? token = null, string? username = null, string? password = null)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = new HttpResponseMessage();

                    if (username != null && password != null)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{username}:{password}")));
                        response = await client.PostAsync(url, null);
                    }
                    else if (token != null)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
                        response = await client.GetAsync(url);
                    }


                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }

                    else
                    {
                        Debug.WriteLine($"Error in GetDataByUrl: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetDataByUrl: {ex.Message}");
            }

            return string.Empty;
        }

        protected async Task<HttpStatusCode> SendDataByUrl(HttpMethod method, string url, AuthToken token, string data)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("URL cannot be null or empty.", nameof(url));

            using (HttpClient client = new HttpClient())
            {
                if (token != null)
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.access_token);
                }
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage result;

                    switch (method.Method.ToUpper())
                    {
                        case "POST":
                            result = await client.PostAsync(url, content);
                            break;
                        case "PATCH":
                            result = await client.PatchAsync(url, content);
                            break;
                        default:
                            throw new ArgumentException("Unsupported HTTP method.", nameof(method));
                    }

                    result.EnsureSuccessStatusCode();
                    return result.StatusCode;
                }
                catch (HttpRequestException ex)
                {
                    Debug.WriteLine($"HTTP request error: {ex.Message}");
                    return HttpStatusCode.InternalServerError;
                }
                catch (TaskCanceledException ex)
                {
                    Debug.WriteLine($"Request timeout: {ex.Message}");
                    return HttpStatusCode.RequestTimeout;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Unexpected error: {ex.Message}");
                    return HttpStatusCode.InternalServerError;
                }
            }
        }

        protected async Task<HttpStatusCode> DeleteDataByUrl(string url, AuthToken token)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.access_token);
                    HttpResponseMessage response = await client.DeleteAsync(url);

                    return response.StatusCode;
                }
            }
            catch (HttpRequestException)
            {
                return HttpStatusCode.InternalServerError;
            }
            catch (TaskCanceledException)
            {
                return HttpStatusCode.RequestTimeout;
            }
            catch (Exception)
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
