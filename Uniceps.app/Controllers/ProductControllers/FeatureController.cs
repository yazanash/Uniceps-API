using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.DTOs.ProductDtos.ProductFeatureDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Products;
using Uniceps.Entityframework.Services.ProductServices;

namespace Uniceps.app.Controllers.ProductControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class FeatureController : ControllerBase
    {
        private readonly IProductRelatedDataService<ProductFeature> _featureService;
        private readonly IMapperExtension<ProductFeature, ProductFeatureDto, ProductFeatureCreationDto> _mapper;

        public FeatureController(IProductRelatedDataService<ProductFeature> featureService, IMapperExtension<ProductFeature, ProductFeatureDto, ProductFeatureCreationDto> mapper)
        {
            _featureService = featureService;
            _mapper = mapper;
        }
        [HttpGet("product/{productId}")]
        [AllowAnonymous] // مسموح للكل يشوف الميزات
        public async Task<IActionResult> GetByProduct(int productId)
        {
            var features = await _featureService.GetAllByProductId(productId);
            return Ok(features.Select(x=> _mapper.ToDto(x)).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductFeatureCreationDto dto)
        {
            var feature = _mapper.FromCreationDto(dto);
            var result = await _featureService.Create(feature);
            return CreatedAtAction(nameof(GetByProduct), new { productId = result.ProductId }, _mapper.ToDto(result) );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductFeatureCreationDto dto)
        {
            var feature = _mapper.FromCreationDto(dto);
            feature.Id = id;
            await _featureService.Update(feature);
            return Ok("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _featureService.Delete(id);
            return Ok("Deleted Successfully");
        }
    }
}
