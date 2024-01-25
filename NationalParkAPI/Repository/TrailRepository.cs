using Microsoft.EntityFrameworkCore;
using NationalParkAPI.Data;
using NationalParkAPI.Models;
using NationalParkAPI.Repository.IRepository;

namespace NationalParkAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;

        public TrailRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateTrail(Trail trail)
        {
            _db.trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.trails.Remove(trail);
            return Save();
        }

        public Trail GetTrailById(int trailId)
        {
            return _db.trails.Include(c => c.NationalPark).FirstOrDefault(q => q.Id == trailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.trails.Include(c => c.NationalPark).OrderBy(q => q.Name).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
        public bool TrailExist(string name)
        {
            bool value = _db.trails.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }
        public bool TrailExist(int id)
        {
            return _db.trails.Any(s => s.Id == id);
        }

        public bool UpdateTrail(Trail trail)
        {
           _db.trails.Update(trail);
            return Save();
        }
        public ICollection<Trail>GetTrailsInNationalPark(int nationalPark)
        {
            return _db.trails.Include(c => c.NationalPark).Where(c => c.NationalParkId == nationalPark).ToList();
        }
    }
}
