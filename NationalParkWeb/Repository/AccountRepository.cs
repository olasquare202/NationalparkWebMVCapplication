using NationalParkWeb.Models;
using NationalParkWeb.Repository.IRepository;
using Newtonsoft.Json;
using System.Text;

namespace NationalParkWeb.Repository
{
    public class AccountRepository : Repository<User>, IAccountRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public AccountRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    
public async Task<User> LoginAsync(string url, User objToCreate)
        {
            //create variable request
            var request = new HttpRequestMessage(HttpMethod.Post, url);//How to use Generic mtd to impliment Create
            //check d new obj to be created, if its not null
            if (objToCreate != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return new User();
            }
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            //check response status code
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(jsonString);

            }
            else
            {
                return new User();
            }
        }

        public async Task<bool> RegisterAsync(string url, User objToCreate)
        {
            //create variable request
            var request = new HttpRequestMessage(HttpMethod.Post, url);//How to use Generic mtd to impliment Create
            //check d new obj to be created, if its not null
            if (objToCreate != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            //check response status code
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
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
