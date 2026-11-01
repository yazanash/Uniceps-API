using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniceps.app.DTOs.ProductDtos;
using Uniceps.app.DTOs.ProductDtos.FAQDtos;
using Uniceps.app.DTOs.ProductDtos.ProductFeatureDtos;
using Uniceps.app.DTOs.ProductDtos.UserStepDtos;
using Uniceps.app.DTOs.ReleaseDtos;
using Uniceps.app.DTOs.SystemSubscriptionDtos;
using Uniceps.app.DTOs.UserDeviceDto;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Products;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;
using Uniceps.Entityframework.Services.ProductServices;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;

namespace Uniceps.app.Controllers.ProductControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductLandingController : ControllerBase
    {
        private readonly IProductDataService _productDataService;
        private readonly ISiteSettingsService _settingsService;
        private readonly IProductRelatedDataService<ProductFeature> _featureService;
        private readonly IProductRelatedDataService<FrequentlyAskedQuestion> _faqService;
        private readonly IProductRelatedDataService<UserStep> _stepService;
        private readonly IReleaseDataService _releaseService;
        private readonly IPlanDataService _planDataService;
        private readonly IMembershipDataService _membershipDataService;
     

        public ProductLandingController(IProductDataService productDataService,
            ISiteSettingsService settingsService,
            IProductRelatedDataService<ProductFeature> featureService,
            IProductRelatedDataService<FrequentlyAskedQuestion> faqService,
            IProductRelatedDataService<UserStep> stepService,
            IReleaseDataService releaseService, IPlanDataService planDataService,
            IMembershipDataService membershipDataService)
        {
            _productDataService = productDataService;
            _settingsService = settingsService;
            _featureService = featureService;
            _faqService = faqService;
            _stepService = stepService;
            _releaseService = releaseService;
            _planDataService = planDataService;
            _membershipDataService = membershipDataService;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetFullProductData(string productId)
        {
            try
            {
                var response = new ProductLandingDto();
                var product = await _productDataService.GetBySlug(productId);
                var settings = await _settingsService.Get();
                var features = await _featureService.GetAllByProductId(product.Id);
                var faqs = await _faqService.GetAllByProductId(product.Id);
                var steps = await _stepService.GetAllByProductId(product.Id);
                var releases = await _releaseService.GetLatestReleasesAsync(product.Id);
                var plans = await _planDataService.GetPlansForApp(product.Id);


                response.SiteSettings = settings;
                response.Features = features.Select(x=> new ProductFeatureDto(x)).ToList();
                response.FAQs = faqs.Select(x => new FrequentlyAskedQuestionDto(x)).ToList(); 
                response.Steps = steps.Select(x =>new UserStepDto(x)).ToList(); 
                response.LatestReleases = releases.Select(x => new ReleaseDto(x)).ToList();
                response.PricingPlans = plans.Select(x => new PlanItemDto(x)).ToList();
                response.Product = new ProductDto(product);

                //if (User.Identity?.IsAuthenticated == true)
                //{
                //    string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;

                //    bool hasHistory = await _membershipDataService.HasUsedTrialForProduct(userId, product.Id);
                        
                //    if (hasHistory)
                //    {
                //        foreach (var plan in response.PricingPlans)
                //        {
                //            plan.PlanItems = plan.PlanItems.Where(i => !i.IsFree).ToList();
                //        }
                //        response.PricingPlans = response.PricingPlans.Where(p => p.PlanItems.Any()).ToList();
                //    }
                //}

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
