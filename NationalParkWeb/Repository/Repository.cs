using NationalParkWeb.Repository.IRepository;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
//using Newtonsoft.Json;
//using System.Text.Json.Serialization;

namespace NationalParkWeb.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _clientFactory;

        //make the Http call using IClientFactory
        public Repository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<bool> CreateAsync(string url, T objToCreate, string token="")
        {//create variable request
            var request = new HttpRequestMessage(HttpMethod.Post, url);//How to use Generic mtd to impliment Create
            //check d new obj to be created, if its not null
            if (token != null && objToCreate != null) 
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            if(token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            //check response status code
            if(response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else 
            { 
                return false; 
            }
        }

        public async Task<bool> DeleteAsync(string url, int Id, string token = "")
        {
            //create variable request
            var endPoint = $"{url}?Id={Id}";
            //var request = new HttpRequestMessage(HttpMethod.Delete, url + Id);
            var request = new HttpRequestMessage(HttpMethod.Delete, endPoint);
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url, string token = "")
        {
            //create variable request
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<T> GetAsync(string url, int Id, string token = "")
        {
            //create variable request
            var request = new HttpRequestMessage(HttpMethod.Get, url + Id);
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateAsync(string url, T objToUpdate, string token = "")
        {
            //create variable request
            var request = new HttpRequestMessage(HttpMethod.Patch, url);//How to use Generic mtd to impliment Create
            //check d new obj to be created, if its not null
            if (objToUpdate != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(objToUpdate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            //check response status code
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
