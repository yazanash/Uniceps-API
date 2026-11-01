using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.DTOs.ProductDtos;
using Uniceps.app.DTOs.ProductDtos.ProductFeatureDtos;
using Uniceps.app.Extensions;
using Uniceps.app.Services;
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
        private readonly ExerciseImageService _imageService;

        public FeatureController(IProductRelatedDataService<ProductFeature> featureService, ExerciseImageService imageService)
        {
            _featureService = featureService;
            _imageService = imageService;
        }
        [HttpGet("product/{productId}")]
        [AllowAnonymous] 
        public async Task<IActionResult> GetByProduct(int productId)
        {
            var features = await _featureService.GetAllByProductId(productId);
            return Ok(features.Select(x=> new ProductFeatureDto(x)).ToList());
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] ProductFeatureCreationDto dto)
        {
            var feature = dto.ToModel();
            if(dto.FeatureImage!=null && dto.FeatureImage.Length > 0)
            {
                string imageUrl = await _imageService.SaveImageAsWebP(dto.FeatureImage,
                    feature.Title.ToUrlSlug(),
                    "features",
                    1200);
                feature.ImageUrl = imageUrl;
            }
            var result = await _featureService.Create(feature);
            return CreatedAtAction(nameof(GetByProduct), new { productId = result.ProductId }, new ProductFeatureDto(result) );
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductFeatureCreationDto dto)
        {
            try
            {
                var existingFeature = await _featureService.Get(id);
                if (existingFeature == null)
                    return NotFound("Feature not found");

                string imageUrl = existingFeature.ImageUrl;

                if (dto.FeatureImage != null && dto.FeatureImage.Length > 0)
                {
                    imageUrl = await _imageService.SaveImageAsWebP(
                        dto.FeatureImage,
                        dto.Title.ToUrlSlug(),
                        "features",
                        1200
                    );

                    if (!string.IsNullOrEmpty(existingFeature.ImageUrl))
                    {
                        _imageService.DeleteImage(existingFeature.ImageUrl);
                    }
                }

                var feature = dto.ToModel();
                feature.Id = id;
                feature.ImageUrl = imageUrl; 

                await _featureService.Update(feature);

                return Ok(new ProductFeatureDto(feature));
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating feature: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _featureService.Delete(id);
            return Ok("Deleted Successfully");
        }
    }
}
