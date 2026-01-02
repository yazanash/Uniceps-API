using Uniceps.app.DTOs.ProductDtos.ProductFeatureDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.Extensions.ProductMappers
{
    public class ProductFeatureMapperExension : IMapperExtension<ProductFeature, ProductFeatureDto, ProductFeatureCreationDto>
    {
        public ProductFeature FromCreationDto(ProductFeatureCreationDto data)
        {
            return new ProductFeature 
            {
                Title = data.Title,
                Description = data.Description,
                TitleAr = data.TitleAr,
                DescriptionAr = data.DescriptionAr,
                ProductId =data.ProductId,
            };
        }

        public ProductFeatureDto ToDto(ProductFeature data)
        {
            return new ProductFeatureDto
            {
                Id=data.Id,
                Description = data.Description,
                ProductId = data.ProductId,
                Title = data.Title,
                TitleAr = data.TitleAr,
                DescriptionAr = data.DescriptionAr,
            };
        }
    }
}
