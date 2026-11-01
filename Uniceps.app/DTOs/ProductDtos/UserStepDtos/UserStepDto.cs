using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.DTOs.ProductDtos.UserStepDtos
{
    public class UserStepDto
    {
        public UserStepDto(UserStep data)
        {

            Id = data.Id;
            ProductId = data.ProductId;
            StepNumber = data.StepNumber;
            Description = data.Description;
            ImageUrl = data.ImageUrl;
            Title = data.Title;
            DescriptionAr = data.DescriptionAr;
            TitleAr = data.TitleAr;
        }
        public int Id { get; set; }
        public int StepNumber { get; set; } // ترتيب الخطوة
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } // صورة توضيحية للخطوة
        public int ProductId { get; set; }
    }
}
