using NationalParkAPI.Models;
using NationalParkAPI.Models.Dtos;

namespace NationalParkAPI.Repository.IRepository
{
    public interface ITrailRepository
    {
        ICollection<Trail> GetTrails();
        ICollection<Trail>GetTrailsInNationalPark(int nationalParkId);
        Trail GetTrailById(int trailId);
        bool TrailExist(string name);
        bool TrailExist(int id);
        bool CreateTrail(Trail trail);
        bool UpdateTrail(Trail trail);
        bool DeleteTrail(Trail trail);
        bool Save();
       // bool CreateTrail(TrailDto trailObj);
    }
}
