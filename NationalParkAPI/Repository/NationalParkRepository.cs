using NationalParkAPI.Data;
using NationalParkAPI.Models;
using NationalParkAPI.Repository.IRepository;
using System.Reflection.Metadata.Ecma335;

namespace NationalParkAPI.Repository
{
    public class NationalParkRepository : INationalParkRepository
    {
        //talk to the DB direct here
        private readonly ApplicationDbContext _db;

        //Dipendency injection, we inject ApplicationDbContext
        public NationalParkRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _db.nationalParks.Add(nationalPark);
            return Save();
        }


        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _db.nationalParks.Remove(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
           return _db.nationalParks.FirstOrDefault(a => a.Id == nationalParkId);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _db.nationalParks.OrderBy(a => a.Name).ToList();
        }

        public bool NationalParkExist(string name)
        {
            bool value = _db.nationalParks.Any(a => a.Name.ToLower().Trim() == name.ToLower().Trim());
            return value;
        }

        public bool NationalParkExist(int id)
        {
            return _db.nationalParks.Any(a =>a.Id == id);   
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _db.nationalParks.Update(nationalPark);
            return Save();
        }
    }
}
