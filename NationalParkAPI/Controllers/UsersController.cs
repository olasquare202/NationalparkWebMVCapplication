using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NationalParkAPI.Models;
using NationalParkAPI.Models.Dtos;
using NationalParkAPI.Repository.IRepository;

namespace NationalParkAPI.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/users")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "NationalParkOpenAPISpecNP")]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]//it's generic to all d methods i.e it applies to all methods here

    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        [AllowAnonymous]    
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticationModel model)
        {
            var user = _userRepo.Authenticate(model.UserName, model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or Password is incorrect" });
            }
            return Ok(user);
        }
        [AllowAnonymous]
        [HttpPost("register")]
        //public IActionResult Register([FromBody] User model)
        public IActionResult Register([FromBody] AuthenticationModel model)
        {
            bool ifUserNameUnique = _userRepo.IsUniqueUser(model.UserName);
            if (!ifUserNameUnique)
            {
                return BadRequest(new { Message = "Username already exists" });
            }
            //var user = _userRepo.Register(model.UserName, model.Password, model.Role);
            var user = _userRepo.Register(model.UserName, model.Password);
            //var user = _userRepo.Register(model);
            if (user == null)
            {
                return BadRequest(new { message = "Error while registering" });
            }
            return Ok(user);
        }
    }
}
