using Uniceps.Entityframework.Models.Products;

namespace Uniceps.app.DTOs.ProductDtos.UserStepDtos
{
    public class UserStepCreationDro
    {
        public int StepNumber { get; set; } // ترتيب الخطوة
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } // صورة توضيحية للخطوة
        public int ProductId { get; set; }
        public UserStep ToModel()
        {
            return new UserStep
            {
                ProductId = ProductId,
                StepNumber = StepNumber,
                Description = Description,
                Title = Title,
                DescriptionAr = DescriptionAr,
                TitleAr = TitleAr,

                ImageUrl = ImageUrl,
            };
        }

    }
}
