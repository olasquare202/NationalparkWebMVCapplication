using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NationalParkWeb.Models;
using NationalParkWeb.Repository.IRepository;

namespace NationalParkWeb.Controllers
{
    [Authorize]
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _nationalParkRepository;

        public NationalParkController(INationalParkRepository nationalParkRepository)
        {
            _nationalParkRepository = nationalParkRepository;
        }
        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }

        [Authorize(Roles ="Admin")]//You are only authorize if your Role is admin
        public async Task <IActionResult> Upsert(int ? id)//if u are Updating NatnaPak, id is needed but not needed for creating NatnaPak
        {
            NationalPark obj = new NationalPark();
            if(id == null)
            {
                //This will be True for Insert or Create
                return View(obj);
            }
            //The flow will come here for Update
            obj = await _nationalParkRepository.GetAsync(StaticDetails.NationalParkAPIPathById, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark obj)
        {
           // if(ModelState.IsValid)
            //{
                //Get the uploaded file
                var files = HttpContext.Request.Form.Files;
                if(files.Count > 0)// if it true convert D file to string i.e Array of byte
                {
                    byte[] p1 = null;//initialize d string conversion
                    using (var fs1 = files[0].OpenReadStream())//we read d file using OpenRead Stream
                    {
                        using var ms1 = new MemoryStream();
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();//we convert d string to array of byte 
                    }
                    obj.Picture = p1;//p1 is d byte of array
                }
                else
                {
                    var objFromDb = await _nationalParkRepository.GetAsync(StaticDetails.NationalParkAPIPathById, obj.Id, HttpContext.Session.GetString("JWToken"));
                    obj.Picture = objFromDb.Picture;
                }
                if(obj.Id == 0)
                {
                    await _nationalParkRepository.CreateAsync(StaticDetails.NationalParkAPIPathByIdCreate, obj, HttpContext.Session.GetString("JWToken"));
                    //return Json(new { success = true, message = "Create Successful" });
                }
                else
                {
                    await _nationalParkRepository.UpdateAsync(StaticDetails.NationalParkAPIPathById + obj.Id, obj, HttpContext.Session.GetString("JWToken"));
                    //return Json(new { success = true, message = "Update Successful" });
                }
                //return RedirectToAction("Index");
                //return RedirectToAction(nameof(Index));
           // }
            //else
            {
                return RedirectToAction("Index");
               // return View(obj);
            }
        }
        public async Task<IActionResult> GetAllNationalPark()
        {
            return Json(new { data = await _nationalParkRepository.GetAllAsync(StaticDetails.NationalParkAPIPathGetAll, HttpContext.Session.GetString("JWToken")) });
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]//You are only authorize if your Role is admin
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _nationalParkRepository.DeleteAsync(StaticDetails.NationalParkAPIPathByIdDelete, id, HttpContext.Session.GetString("JWToken"));
            //var status = await _nationalParkRepository.DeleteAsync("https://localhost:7186/api/v1/nationalParks/DeleteNationalPark/", id);

            if (status)
            {
                return Json(new { success = true, message="Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });

        }
        

    }
}
