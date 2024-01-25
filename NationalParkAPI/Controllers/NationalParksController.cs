using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NationalParkAPI.Models.Dtos;
using NationalParkAPI.Models;
using NationalParkAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;

namespace NationalParkAPI.Controllers
{
    //[Route("api/[controller]")]
   
    [Route("api/v{version:apiVersion}/nationalParks")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "NationalParkOpenAPISpecNP")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]//it's generic to all d methods i.e it applies to all methods here
   
    public class NationalParksController : ControllerBase
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;

        public NationalParksController(INationalParkRepository nationalParkRepository, IMapper mapper)
        {
            _nationalParkRepository = nationalParkRepository;
            _mapper = mapper;
        }
        //Learn how to use XML comment to improve your documentation by pressing ///

        /// <summary>
        /// Get list of all the national parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetNationalParks()
        {
            var objList = _nationalParkRepository.GetNationalParks();
            //we expose the Dto(NationalParkDto) not the Model class(NationalPark)
            //i.e we get the List from Dto not Model class using automapper
            var objDto = new List<NationalParkDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(obj));//use automapper
            }
            return Ok(objList);
        }

        /// <summary>
        /// Get a single national park by Id
        /// </summary>
        /// <param name="nationalParkId">Enter the Id of the national park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type =typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _nationalParkRepository.GetNationalPark(nationalParkId);
            if (obj == null)
            {
                return NotFound();
            }

            //if obj is found, convert the obj to Dto using automapper b4 return
            //Automapper made it simple with just a line of code
            var objDto = _mapper.Map<NationalParkDto>(obj);//Alternatively
            //var objDto = new NationalParkDto()
            //{
            //    Created = objList1.Created,
            //    Id = objList1.Id,
            //    Name = objList1.Name,
            //    State = objList1.State,
            //}
            //return Ok(objDto);
            return Ok(objDto);
        }


        /// <summary>
        /// Create new national park
        /// </summary>
        /// <param name="nationalParkDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateNationalPark")]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            //check if obj is null
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }
            //check if obj exist by Name
            if (_nationalParkRepository.NationalParkExist(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "Nataional Park Exists!");
                return StatusCode(404, ModelState);
            }
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //Hence convert d Dto to model class using automapper i.e
            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_nationalParkRepository.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkObj.Id }, nationalParkObj);
        }


        /// <summary>
        /// Update national park
        /// </summary>
        /// <param name="nationalParkId">Enter the Id of the national park you want to update</param>
        /// <param name="nationalParkDto"></param>
        /// <returns></returns>
        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            //check if obj is null OR existingObj Id is not equal to d Id you want to Update
            if (nationalParkDto == null || nationalParkId != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }
            //Hence convert d Dto to model class using automapper i.e
            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);
            if (!_nationalParkRepository.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {nationalParkObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        /// <summary>
        /// Delete national park
        /// </summary>
        /// <param name="nationalParkId">Enter the Id of the national park you want to delete</param>
        /// <returns></returns>
        //[HttpDelete("{nationalParkId}", Name = "DeleteNationalPark")]
        [HttpDelete("DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
        public IActionResult DeleteNationalPark(int Id)
        {
            //check if d nationalPark you want to Delete does not Exist
            if (!_nationalParkRepository.NationalParkExist(Id))
            {
                return NotFound();
            }
            //Hence if it found, we will get it from d Db by Id & delete it
            var nationalParkObj = _nationalParkRepository.GetNationalPark(Id);
            if (!_nationalParkRepository.DeleteNationalPark(nationalParkObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {nationalParkObj.Id}");
                return StatusCode(500, ModelState);
                
            }
             return NoContent();
            
        }
    }
}


