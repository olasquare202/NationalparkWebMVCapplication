using Microsoft.AspNetCore.Mvc;
using NationalParkWeb.Models;
using NationalParkWeb.Models.ViewModel;
using NationalParkWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace NationalParkWeb.Controllers
{
    [Authorize]
    public class TrailController : Controller
    {
        private readonly ITrailRepository _trailRepository;
        private readonly INationalParkRepository _nationalParkRepository;

        public TrailController(INationalParkRepository nationalParkRepository, ITrailRepository trailRepository)
        {
            _trailRepository = trailRepository;
            _nationalParkRepository = nationalParkRepository;
        }
        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Upsert(int? id)//if u are Updating NatnaPak, id is needed but not needed for creating NatnaPak
        {
            //IEnumerable<NationalPark> NPList = await _nationalParkRepository.GetAllAsync(StaticDetails.NationalParkAPIPathGetAll);
            IEnumerable<NationalPark> NPList = await _nationalParkRepository.GetAllAsync("https://localhost:7186/api/v1/nationalParks", HttpContext.Session.GetString("JWToken"));
            var objVM1 = new TrailVM();
            objVM1.Trail = new Trail();
            objVM1.NationalParkList = new List<SelectListItem>();
            if (NPList != null)
            {
                objVM1.NationalParkList = NPList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });

            }

            if (id == null)
            {
                //This will be True for Insert or Create
                return View(objVM1);
            }
            //The flow will come here for Update
            objVM1.Trail = await _trailRepository.GetAsync(StaticDetails.TrailAPIPath,  id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (objVM1.Trail == null)
            {
                return NotFound();
            }
            return View(objVM1);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailVM obj)
        {
            // if(ModelState.IsValid)
            //{
            //Get the uploaded file

            if (obj.Trail.Id == 0)//TrailAPIPathCreate
            {
                await _trailRepository.CreateAsync(StaticDetails.TrailAPIPathCreate, obj.Trail, HttpContext.Session.GetString("JWToken"));
            }
            else
            {
                await _trailRepository.UpdateAsync(StaticDetails.TrailAPIPath + obj.Trail.Id, obj.Trail, HttpContext.Session.GetString("JWToken"));
            }
            //return RedirectToAction("Index");
            //return RedirectToAction(nameof(Index));
            // }
            //else
            {
                return RedirectToAction(nameof(Index));
                //return RedirectToAction("Index");
                //return View(obj);
            }
        }
        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { data = await _trailRepository.GetAllAsync(StaticDetails.TrailAPIPath, HttpContext.Session.GetString("JWToken")) });
        }
        //[HttpDelete("Delete")]
        [Authorize(Roles = "Admin")]//You are only authorize if your Role is admin
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepository.DeleteAsync(StaticDetails.TrailAPIPathDelete, id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });

        }

    }
}
