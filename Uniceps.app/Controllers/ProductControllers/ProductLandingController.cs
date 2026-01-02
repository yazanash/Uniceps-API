using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniceps.app.DTOs.ProductDtos;
using Uniceps.app.DTOs.ProductDtos.FAQDtos;
using Uniceps.app.DTOs.ProductDtos.ProductFeatureDtos;
using Uniceps.app.DTOs.ProductDtos.UserStepDtos;
using Uniceps.app.DTOs.ReleaseDtos;
using Uniceps.app.DTOs.SystemSubscriptionDtos;
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
        private readonly IMapperExtension<FrequentlyAskedQuestion, FrequentlyAskedQuestionDto, FrequentlyAskedQuestionCreationDto> _faqMapperExtension;
        private readonly IMapperExtension<ProductFeature, ProductFeatureDto, ProductFeatureCreationDto> _featureMapperExtension;
        private readonly IMapperExtension<UserStep, UserStepDto, UserStepCreationDro> _userStepMapperExtension;
        private readonly IMapperExtension<Release, ReleaseDto, ReleaseCreationDto> _releaseMapperExtension;
        private readonly IMapperExtension<PlanModel, PlanDto, PlanCreationDto> _planMapperExtension;
        private readonly IMapperExtension<Product, ProductDto, ProductCreationDto> _productMapper;

        public ProductLandingController(IProductDataService productDataService,
            ISiteSettingsService settingsService,
            IProductRelatedDataService<ProductFeature> featureService,
            IProductRelatedDataService<FrequentlyAskedQuestion> faqService,
            IProductRelatedDataService<UserStep> stepService,
            IReleaseDataService releaseService, IPlanDataService planDataService,
            IMembershipDataService membershipDataService,
            IMapperExtension<FrequentlyAskedQuestion,
                FrequentlyAskedQuestionDto,
                FrequentlyAskedQuestionCreationDto> faqMapperExtension,
            IMapperExtension<ProductFeature, ProductFeatureDto, ProductFeatureCreationDto> featureMapperExtension,
            IMapperExtension<UserStep, UserStepDto, UserStepCreationDro> userStepMapperExtension,
            IMapperExtension<Release, ReleaseDto, ReleaseCreationDto> releaseMapperExtension,
            IMapperExtension<PlanModel, PlanDto, PlanCreationDto> planMapperExtension, IMapperExtension<Product, ProductDto, ProductCreationDto> productMapper)
        {
            _productDataService = productDataService;
            _settingsService = settingsService;
            _featureService = featureService;
            _faqService = faqService;
            _stepService = stepService;
            _releaseService = releaseService;
            _planDataService = planDataService;
            _membershipDataService = membershipDataService;
            _faqMapperExtension = faqMapperExtension;
            _featureMapperExtension = featureMapperExtension;
            _userStepMapperExtension = userStepMapperExtension;
            _releaseMapperExtension = releaseMapperExtension;
            _planMapperExtension = planMapperExtension;
            _productMapper = productMapper;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetFullProductData(int productId)
        {
            try
            {
                var response = new ProductLandingDto();
                var product = await _productDataService.Get(productId);
                // 1. جلب المهام بالتوازي لتحسين الأداء
                var settings = await _settingsService.Get();
                var features = await _featureService.GetAllByProductId(productId);
                var faqs = await _faqService.GetAllByProductId(productId);
                var steps = await _stepService.GetAllByProductId(productId);
                var releases = await _releaseService.GetLatestReleasesAsync(productId);
                var plans = await _planDataService.GetPlansForApp(productId);


                // 2. تعبئة البيانات الأساسية
                response.SiteSettings = settings;
                response.Features = features.Select(x=> _featureMapperExtension.ToDto(x)).ToList();
                response.FAQs = faqs.Select(x => _faqMapperExtension.ToDto(x)).ToList(); 
                response.Steps = steps.Select(x => _userStepMapperExtension.ToDto(x)).ToList(); 
                response.LatestReleases = releases.Select(x => _releaseMapperExtension.ToDto(x)).ToList();
                response.PricingPlans = plans.Select(x => _planMapperExtension.ToDto(x)).ToList();
                response.Product = _productMapper.ToDto(product);
                // 3. منطق جلب الخطط المفلترة (حسب الـ AppId وفحص الفترة التجريبية)

                if (User.Identity?.IsAuthenticated == true)
                {
                    string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;

                    // هل المستخدم اشترك مسبقاً بهذا المنتج؟ (لمنع تكرار الـ IsFree)
                    bool hasHistory = await _membershipDataService.HasUsedTrialForProduct(userId, productId);
                        
                    if (hasHistory)
                    {
                        foreach (var plan in response.PricingPlans)
                        {
                            plan.PlanItems = plan.PlanItems.Where(i => !i.IsFree).ToList();
                        }
                        response.PricingPlans = response.PricingPlans.Where(p => p.PlanItems.Any()).ToList();
                    }
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
