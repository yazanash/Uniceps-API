using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.BusinessLocalDtos;
using Uniceps.app.DTOs.BusinessLocalDtos.BusinessServicesDtos;
using Uniceps.app.Helpers;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.BusinessLocalModels;

namespace Uniceps.app.Controllers.BusinessLocalControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessServiceController : ControllerBase
    {
        private readonly IDataService<BusinessServiceModel> _dataService;
        private readonly IUserQueryDataService<BusinessServiceModel> _userQueryDataService;
        private readonly IMapperExtension<BusinessServiceModel, BusinessServiceDto, BusinessServiceCreationDto> _mapperExtension;

        public BusinessServiceController(IDataService<BusinessServiceModel> dataService, IMapperExtension<BusinessServiceModel, BusinessServiceDto, BusinessServiceCreationDto> mapperExtension, IUserQueryDataService<BusinessServiceModel> userQueryDataService)
        {
            _dataService = dataService;
            _mapperExtension = mapperExtension;
            _userQueryDataService = userQueryDataService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (!HttpContext.IsBusinessUser())
            {
                return Forbid();
            }
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            IEnumerable<BusinessServiceModel> players = await _userQueryDataService.GetAllByUser(userId);
            return Ok(players.Select(x => _mapperExtension.ToDto(x)).ToList());
        }
        [HttpPost]
        public async Task<IActionResult> Create(BusinessServiceCreationDto playerModelCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (!HttpContext.IsBusinessUser())
            {
                return Forbid();
            }
            if (playerModelCreationDto == null)
                return BadRequest("Exercise data is missing.");

            BusinessServiceModel service = _mapperExtension.FromCreationDto(playerModelCreationDto);

            service.BusinessId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _dataService.Create(service);
            return Ok(_mapperExtension.ToDto(service));
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BusinessServiceCreationDto playerModelCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (!HttpContext.IsBusinessUser())
            {
                return Forbid();
            }
            if (playerModelCreationDto == null)
                return BadRequest("Exercise data is missing.");

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            BusinessServiceModel playerModel = await _dataService.Get(playerModelCreationDto.ApiId);
            BusinessServiceModel newPlayerModel = _mapperExtension.FromCreationDto(playerModelCreationDto);
            newPlayerModel.Id = playerModel.Id;
            //newPlayerModel.UserId = userId;
            await _dataService.Update(newPlayerModel);
            return Ok("Updated successfully");
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            await _dataService.Delete(id);
            return Ok("Deleted successfully");
        }
    }
}
