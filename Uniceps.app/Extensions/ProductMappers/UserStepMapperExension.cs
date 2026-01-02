using Uniceps.app.DTOs.ProductDtos.UserStepDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.Extensions.ProductMappers
{
    public class UserStepMapperExension : IMapperExtension<UserStep, UserStepDto, UserStepCreationDro>
    {
        public UserStep FromCreationDto(UserStepCreationDro data)
        {
            return new UserStep
            {
                ProductId = data.ProductId,
                StepNumber = data.StepNumber,
                Description = data.Description,
                Title = data.Title,
                DescriptionAr = data.DescriptionAr,
                TitleAr = data.TitleAr,

                ImageUrl = data.ImageUrl,
            };
        }

        public UserStepDto ToDto(UserStep data)
        {
            return new UserStepDto
            {
                Id = data.Id,
                ProductId = data.ProductId,
                StepNumber = data.StepNumber,
                Description = data.Description,
                ImageUrl = data.ImageUrl,
                Title = data.Title,
                DescriptionAr = data.DescriptionAr,
                TitleAr = data.TitleAr
            };
        }
    }
}
