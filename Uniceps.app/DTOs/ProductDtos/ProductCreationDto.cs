using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.DTOs.ProductDtos
{
    public class ProductCreationDto
    {
        public string Name { get; set; } = "";
        public string NameAr { get; set; } = "";
        public Platform Platform { get; set; }
        public string Description { get; set; } = "";
        public string DescriptionAr { get; set; } = "";
        public IFormFile? HeroImage { get; set; }
        public int AppId { get; set; }
        public Product ToModel()
        {
            Product product = new Product()
            {
                AppId = AppId,
                Description = Description,
                Name = Name,
                DescriptionAr = DescriptionAr,
                NameAr = NameAr,
                Platform = Platform,
            };
            return product;
        }
    }
}
