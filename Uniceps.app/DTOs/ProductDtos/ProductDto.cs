using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.DTOs.ProductDtos
{
    public class ProductDto
    {
        public ProductDto(Product data)
        {

            Name = data.Name;
            Id = data.Id;
            Description = data.Description;
            HeroImage = data.HeroImage;
            Platform = data.Platform;
            AppId = data.AppId;
            DescriptionAr = data.DescriptionAr;
            NameAr = data.NameAr;
            Slug = data.Slug;

        }
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string NameAr { get; set; } = "";
        public Platform Platform { get; set; }
        public string Description { get; set; } = "";
        public string DescriptionAr { get; set; } = "";
        public string? HeroImage { get; set; } = "";
        public int AppId { get; set; }
        public string Slug { get; set; } = "";
    }
}
