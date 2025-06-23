using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.BusinessLocalDtos;
using Uniceps.app.DTOs.ProfileDtos;
using Uniceps.app.Helpers;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.BusinessLocalModels;
using Uniceps.Entityframework.Models.Profile;

namespace Uniceps.app.Controllers.BusinessLocalControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IDataService<PlayerModel> _dataService;
        private readonly IMapperExtension<PlayerModel, PlayerModelDto, PlayerModelCreationDto> _mapperExtension;

        public PlayerController(IDataService<PlayerModel> dataService, IMapperExtension<PlayerModel, PlayerModelDto, PlayerModelCreationDto> mapperExtension)
        {
            _dataService = dataService;
            _mapperExtension = mapperExtension;
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
    }
}
