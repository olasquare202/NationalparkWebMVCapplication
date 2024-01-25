using Microsoft.EntityFrameworkCore;
using NationalParkAPI.Models;

namespace NationalParkAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
             
        }
       public DbSet<NationalPark> nationalParks { get; set; }
        public DbSet<Trail> trails { get; set; }
        public DbSet<User> users { get; set; }
    }
}
