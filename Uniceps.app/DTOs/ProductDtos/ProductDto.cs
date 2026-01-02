using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.DTOs.ProductDtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string NameAr { get; set; } = "";
        public Platform Platform { get; set; }
        public string Description { get; set; } = "";
        public string DescriptionAr { get; set; } = "";
        public string? HeroImage { get; set; } = "";
        public int AppId { get; set; }
    }
}
