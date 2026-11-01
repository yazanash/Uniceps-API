using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.DTOs.ProductDtos.ProductFeatureDtos
{
    public class ProductFeatureDto
    {
        public ProductFeatureDto (ProductFeature data)
        {
            Id = data.Id;
            Description = data.Description;
            ProductId = data.ProductId;
            Title = data.Title;
            TitleAr = data.TitleAr;
            DescriptionAr = data.DescriptionAr;
            ImageUrl = data.ImageUrl;
        }
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        public int ProductId { get; set; }
    }
}
