using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.DTOs.ProductDtos.ProductFeatureDtos
{
    public class ProductFeatureCreationDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public IFormFile? FeatureImage { get; set; }
        public ProductFeature ToModel()
        {
            return new ProductFeature
            {
                Title = Title,
                Description = Description,
                TitleAr = TitleAr,
                DescriptionAr = DescriptionAr,
                ProductId = ProductId,
            };
        }
    }
}
