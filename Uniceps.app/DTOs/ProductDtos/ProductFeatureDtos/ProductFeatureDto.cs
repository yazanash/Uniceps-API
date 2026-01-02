namespace Uniceps.app.DTOs.ProductDtos.ProductFeatureDtos
{
    public class ProductFeatureDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public int ProductId { get; set; }
    }
}
