using NationalParkAPI.Models;
using NationalParkAPI.Models.Dtos;

namespace NationalParkAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);//check if d User is Unique
        //User Authenticate(string username, string password);//to authenticate d User
        User Authenticate(string username, string password);//to authenticate d User
                                                            // User Register(string username, string password, string role);//to Register d User

        //User Register(AuthenticationModel model);//to Register d User
        User Register(string username, string password);//to Register d User
    }
}
