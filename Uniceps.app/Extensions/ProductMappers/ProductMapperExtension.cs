using Uniceps.app.DTOs.ProductDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.Extensions.ProductMappers
{
    public class ProductMapperExtension : IMapperExtension<Product, ProductDto, ProductCreationDto>
    {
        public Product FromCreationDto(ProductCreationDto data)
        {
            Product product = new Product()
            {
                AppId = data.AppId,
                Description = data.Description,
                Name = data.Name,
                DescriptionAr = data.DescriptionAr,
                NameAr = data.NameAr,
                Platform = data.Platform
            };
            return product;
        }

        public ProductDto ToDto(Product data)
        {
            ProductDto productDto = new ProductDto()
            {
                Name = data.Name,
                Id = data.Id,
                Description = data.Description,
                HeroImage = data.HeroImage,
                Platform = data.Platform,
                 AppId = data.AppId,
                DescriptionAr = data.DescriptionAr,
                NameAr = data.NameAr
            };
            return productDto;
        }
    }
}
