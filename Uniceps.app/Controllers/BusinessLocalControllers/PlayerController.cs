using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.BusinessLocalDtos;
using Uniceps.app.DTOs.ProfileDtos;
using Uniceps.app.Helpers;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.BusinessLocalModels;
using Uniceps.Entityframework.Models.Profile;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.Controllers.BusinessLocalControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IDataService<PlayerModel> _dataService;
        private readonly IUserQueryDataService<PlayerModel> _userQueryDataService;
        private readonly IMapperExtension<PlayerModel, PlayerModelDto, PlayerModelCreationDto> _mapperExtension;

        public PlayerController(IDataService<PlayerModel> dataService, IMapperExtension<PlayerModel, PlayerModelDto, PlayerModelCreationDto> mapperExtension, IUserQueryDataService<PlayerModel> userQueryDataService)
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
            IEnumerable<PlayerModel> players = await _userQueryDataService.GetAllByUser(userId);
            return Ok(players.Select(x => _mapperExtension.ToDto(x)).ToList());
        }
        [HttpPost]
        public async Task<IActionResult> Create(PlayerModelCreationDto playerModelCreationDto)
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

            PlayerModel player = _mapperExtension.FromCreationDto(playerModelCreationDto);

            player.BusinessId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _dataService.Create(player);
            return Ok(_mapperExtension.ToDto(player));
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PlayerModelCreationDto playerModelCreationDto)
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
            PlayerModel playerModel = await _dataService.Get(playerModelCreationDto.ApiId);
            PlayerModel newPlayerModel = _mapperExtension.FromCreationDto(playerModelCreationDto);
            newPlayerModel.Id = playerModel.Id;
            newPlayerModel.UserId = userId;
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
