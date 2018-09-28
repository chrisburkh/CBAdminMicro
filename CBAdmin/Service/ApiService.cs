using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CBAdmin.Models;
using Http;
using Newtonsoft.Json;

namespace CBAdmin.Service
{
    public class ApiService<T> : IApiService<T>
    {

        private readonly IHttpClient _apiClient;
        private string _baseUrl;

        public ApiService(IHttpClient httpClient)
        {
            _apiClient = httpClient;
        }


        public async Task Create(T entity)
        {
            var response = await _apiClient.PostAsync("http://localhost:8081/api/" + _baseUrl, entity);
            response.EnsureSuccessStatusCode();
        }



        public async Task Delete(string id)
        {
            var response = await _apiClient.DeleteAsync("http://localhost:8081/api/" + _baseUrl + "/" + id);
            response.EnsureSuccessStatusCode();
        }

        public async Task<T> Get(string id)
        {
            var dataString = await _apiClient.GetStringAsync("http://localhost:8081/api/" + _baseUrl + "/" + id);
            return JsonConvert.DeserializeObject<T>(dataString);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var dataString = await _apiClient.GetStringAsync("http://localhost:8081/api/" + _baseUrl);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(dataString);
        }


        public async Task<IEnumerable<T>> GetAll(string searchstring)
        {
            var dataString = await _apiClient.GetStringAsync("http://localhost:8081/api/" + _baseUrl + "?search=" + searchstring);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(dataString);
        }

        public async Task Write(T student)
        {
            var response = await _apiClient.PutAsync("http://localhost:8081/api/" + _baseUrl, student);
            response.EnsureSuccessStatusCode();
        }

        public void SetBaseUrl(string url)
        {
            _baseUrl = url;
        }
    }
}
