using NationalParkWeb.Models;
using NationalParkWeb.Repository.IRepository;

namespace NationalParkWeb.Repository
{
   
    public class TrailRepository : Repository<Trail>, ITrailRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public TrailRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
