namespace Uniceps.app.DTOs.ProductDtos.UserStepDtos
{
    public class UserStepDto
    {
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
