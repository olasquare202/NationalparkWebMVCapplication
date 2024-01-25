using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NationalParkAPI.Data;
using NationalParkAPI.Models;
using NationalParkAPI.Models.Dtos;
using NationalParkAPI.Repository.IRepository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NationalParkAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        //private readonly IOptions<AppSettings> _appSettings;
        private readonly AppSettings _appSettings;

        //public UserRepository(ApplicationDbContext db, IOptions<AppSettings> appsettings, AppSettings appSettings)
        public UserRepository(ApplicationDbContext db, IOptions<AppSettings> appSettings)
        {
            _db = db;
            //_appSettings = appsettings;
            _appSettings = appSettings.Value;
            //.UseServiceProviderFactory(new LightInjectServiceProviderFactory())
        }
        //public User Authenticate(string username, string password)
        public User Authenticate(string username, string password)
        {
            var user = _db.users.FirstOrDefault(x => x.UserName == username && x.Password == password);
            //user not found
            if (user == null)
            {
                return null;
            }
            //Map user to userDto
            //var userDto = new User
            //{
            //   UserName = user.UserName,
            //   //Password = user.Password,
            //   Role = user.Role
            //};
            //if user was found, generate JWT Token
            var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_appSettings.Value.Secret);
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),//i just limit it to only one claim(i.e Name) 
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //userDto.Token = tokenHandler.WriteToken(token);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = "";
            //return userDto;
            return user;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.users.FirstOrDefault(x => x.UserName == username);
            //return null if user not found
            if(user == null)
            
                return true;//Dis means that the user Id is unique
            
            return false;

            
        }

        public User Register(string username, string password)
        {
            User userObj = new User()
            {
                UserName = username,
                Password = password,
                //Role     = "Admin"
                Role     = "User"
            };
            _db.users.Add(userObj);
            _db.SaveChanges();
            userObj.Password = "";
            return userObj;
        }

        //User IUserRepository.Register(string username, string password)
        //{
        //    throw new NotImplementedException();
        //}
        //public User Register(AuthenticationModel model)
        //{
        //    User userObj = new User()
        //    {
        //        UserName = model.UserName,
        //        Password = model.Password,
        //       // Role = model.Role

        //    };
        //    _db.users.Add(userObj);
        //    _db.SaveChanges();
        //    userObj.Password = "";
        //    return userObj;
        //}
    }
}
