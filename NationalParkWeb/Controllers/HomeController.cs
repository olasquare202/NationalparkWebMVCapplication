using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NationalParkWeb.Models;
using NationalParkWeb.Models.ViewModel;
using NationalParkWeb.Repository.IRepository;
using System.Diagnostics;
using System.Security.Claims;

namespace NationalParkWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;
        private readonly IAccountRepository _accountRepository;

        public HomeController(ILogger<HomeController> logger, ITrailRepository trailRepo, IAccountRepository accountRepository, INationalParkRepository npRepo)
        {
            _logger = logger;
            _trailRepo = trailRepo;
            _accountRepository = accountRepository;
            _npRepo = npRepo;
        }

        public async Task <IActionResult> Index()
        {
           
           var emptyNationalParkList = new List<NationalPark>();
            var emptyTrailList = new List<Trail>();
            //var nationalParksList = await _npRepo.GetAllAsync(StaticDetails.NationalParkAPIPathGetAll);
            var nationalParksList = await _npRepo.GetAllAsync("https://localhost:7186/api/v1/nationalParks", HttpContext.Session.GetString("JWToken"));

            var trailList = await _trailRepo.GetAllAsync(StaticDetails.TrailAPIPath, HttpContext.Session.GetString("JWToken"));
            IndexVM listOfPaksAndTrails = new IndexVM()
            
            {
                NationalParkList = nationalParksList == null ? emptyNationalParkList :  nationalParksList,
                TrailList = trailList == null ? emptyTrailList : trailList
            };
            return View(listOfPaksAndTrails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult Login()
        {
            User obj = new User();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Login(User obj)
        {
            User objUser = await _accountRepository.LoginAsync(StaticDetails.AccountAPIPath + "authenticate/", obj);
            //check if the user token is null
            if(objUser.Token == null)
            {
                TempData["alert"] = "Username or Password is incorrect "; //We implement Tempdata here
                return View();//i.e Login was not successful
            }
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, objUser.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, objUser.Role));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            //save the token in a session
            HttpContext.Session.SetString("JWToken", objUser.Token);
            TempData["alert"] = "Welcome " + objUser.UserName; //We implement Tempdata here
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Register()
        {
            User obj = new User();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User obj)
        {
            bool result = await _accountRepository.RegisterAsync(StaticDetails.AccountAPIPath + "register/", obj);
            //check if the user token is null
            if (result == false)
            {
                TempData["alert"] = "Enter new Username and Password "; //We implement Tempdata here
                return View();//i.e Login was not successful
            }
            TempData["alert"] = "Registration Successful"; //We implement Tempdata here
            return RedirectToAction("Login");
        }
        public async Task <IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            //save the token in a session
            HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}