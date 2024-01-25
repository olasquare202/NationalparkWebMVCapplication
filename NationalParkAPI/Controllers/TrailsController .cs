using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NationalParkAPI.Models.Dtos;
using NationalParkAPI.Models;
using NationalParkAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;

namespace NationalParkAPI.Controllers
{
    //[Route("api/Trails")]
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "NationalParkOpenAPISpecTrails")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]//it's generic to all d methods i.e it applies to all methods here
    public class TrailsController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepository, IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }
        //Learn how to use XML comment to improve your documentation by pressing ///

        /// <summary>
        /// Get list of all the trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Route("GetTrails")]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetTrails()
        {
            var objList = _trailRepository.GetTrails();
            //we expose the Dto(TrailDto) not the Model class(NationalPark)
            //i.e we get the List from Dto not Model class using automapper
            var objDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));//use automapper
            }
            return Ok(objDto);
        }

        /// <summary>
        /// Get a single trail by Id
        /// </summary>
        /// <param name="trailId">Enter the Id of the trail</param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type =typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles ="Admin")]
        public IActionResult GetTrail(int trailId)
        {
            var obj = _trailRepository.GetTrailById(trailId);
            if (obj == null)
            {
                return NotFound();
            }//if obj is found, convert the obj to Dto using automapper b4 return
            var objDto = _mapper.Map<TrailDto>(obj);
            return Ok(objDto);
        }

        [HttpGet("GetTrailInNationalPark/nationalParkId:int")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetTrailInNationalPark(int nationalParkId)
        {
            var objList = _trailRepository.GetTrailsInNationalPark(nationalParkId);
            if (objList == null)
            {
                return NotFound();
            }//if obj is found, convert the obj to Dto using automapper b4 return
            var objDto = new List<TrailDto>();
            foreach(var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
            
            return Ok(objDto);
        }


        /// <summary>
        /// Create new trail
        /// </summary>
        /// <param name="TrailDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateTrail")]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            //check if obj is null
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }
            //check if obj is exist by Name
            if (_trailRepository.TrailExist(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists!");
                return StatusCode(404, ModelState);
            }
            //if(!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //Hence convert d Dto to model class using automapper i.e
            var trailObj = _mapper.Map<Trail>(trailDto);
            if (!_trailRepository.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetTrail", new { trailId = trailObj.Id }, trailObj);
        }


        /// <summary>
        /// Update trail
        /// </summary>
        /// <param name="trailId">Enter the Id of the trail you want to update</param>
        /// <param name="trailDto"></param>
        /// <returns></returns>
        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailDto)
        {
            //check if obj is null OR existingObj Id is not equal to d Id you want to Update
            if (trailDto == null || trailId != trailDto.Id)
            {
                return BadRequest(ModelState);
            }
            //Hence convert d Dto to model class using automapper i.e
            var trailObj = _mapper.Map<Trail>(trailDto);
            if (!_trailRepository.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


        /// <summary>
        /// Delete taril
        /// </summary>
        /// <param name="trailId">Enter the Id of the trail you want to delete</param>
        /// <returns></returns>
        [HttpDelete("{DeleteTrail}", Name = "DeleteTrail")]
        //[HttpDelete("DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
        public IActionResult DeleteTrail(int Id)
        {
            //check if d nationalPark you want to Delete does not Exist
            if (!_trailRepository.TrailExist(Id))
            {
                return NotFound();
            }
            //Hence if it found, we will get it from d Db by Id & delete it
            var trailObj = _trailRepository.GetTrailById(Id);
            if (!_trailRepository.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {trailObj.Id}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}