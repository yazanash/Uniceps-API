using Uniceps.app.DTOs.ProductDtos.FAQDtos;
using Uniceps.app.DTOs.ProductDtos.ProductFeatureDtos;
using Uniceps.app.DTOs.ProductDtos.UserStepDtos;
using Uniceps.app.DTOs.ReleaseDtos;
using Uniceps.app.DTOs.SystemSubscriptionDtos;
using Uniceps.Entityframework.Models;

namespace Uniceps.app.DTOs.ProductDtos
{
    public class ProductLandingDto
    {
        public ProductDto Product { get; set; } = new();
        public SiteSettings SiteSettings { get; set; } = new();
        public List<ProductFeatureDto> Features { get; set; } = new();
        public List<FrequentlyAskedQuestionDto> FAQs { get; set; } = new();
        public List<UserStepDto> Steps { get; set; } = new();
        public List<PlanDto> PricingPlans { get; set; } = new();
        public List<ReleaseDto> LatestReleases { get; set; } = new();
    }
}
