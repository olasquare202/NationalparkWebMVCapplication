using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NationalParkAPI.Models.Dtos;
using NationalParkAPI.Models;
using NationalParkAPI.Repository.IRepository;

namespace NationalParkAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/nationalParks")]
    [ApiVersion("2.0")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "NationalParkOpenAPISpecV2")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]//it's generic to all d methods i.e it applies to all methods here
    public class NationalParksV2Controller : ControllerBase
    {
        private readonly INationalParkRepository _nationalParkRepository;
        private readonly IMapper _mapper;

        public NationalParksV2Controller(INationalParkRepository nationalParkRepository, IMapper mapper)
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
            var obj = _nationalParkRepository.GetNationalParks().FirstOrDefault();
            
            return Ok(_mapper.Map<NationalParkDto>(obj));
        }

      
    }
}