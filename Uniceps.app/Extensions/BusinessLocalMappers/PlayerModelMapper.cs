using Uniceps.app.DTOs.BusinessLocalDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.BusinessLocalModels;

namespace Uniceps.app.Extensions.BusinessLocalMappers
{
    public class PlayerModelMapper : IMapperExtension<PlayerModel, PlayerModelDto, PlayerModelCreationDto>
    {
        public PlayerModel FromCreationDto(PlayerModelCreationDto data)
        {
            PlayerModel playerModel = new PlayerModel();
            playerModel.Name = data.Name;
            playerModel.Phone = data.Phone;
            playerModel.SubscribeDate = data.SubscribeDate;
            playerModel.SubscribeEndDate = data.SubscribeEndDate;
            playerModel.IsSubscribed = data.IsSubscribed;
            playerModel.Balance = data.Balance;
            playerModel.UserId = data.UserId;
            return playerModel;
        }

        public PlayerModelDto ToDto(PlayerModel data)
        {
            PlayerModelDto playerModelDto = new PlayerModelDto();
            playerModelDto.Id = data.Id;
            playerModelDto.Name = data.Name;
            playerModelDto.Phone = data.Phone;
            playerModelDto.SubscribeDate = data.SubscribeDate;
            playerModelDto.SubscribeEndDate = data.SubscribeEndDate;
            playerModelDto.IsSubscribed = data.IsSubscribed;
            playerModelDto.Balance = data.Balance;

            return playerModelDto;
        }
    }
}
