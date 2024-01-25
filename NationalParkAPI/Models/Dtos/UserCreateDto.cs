using System.ComponentModel.DataAnnotations.Schema;

namespace NationalParkAPI.Models.Dtos
{
    public class UserCreateDto
    {

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        [NotMapped]
        public string Token { get; set; }
    }
}
