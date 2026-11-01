using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using Uniceps.app.DTOs.ProductDtos;
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
    public class ProductController : ControllerBase
    {
        public IProductDataService _dataService;
        private readonly ExerciseImageService _imageService;
        public ProductController(IProductDataService dataService, ExerciseImageService imageService)
        {
            _dataService = dataService;
            _imageService = imageService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _dataService.GetAll();
            return Ok(products.Select(x =>new ProductDto(x)));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _dataService.Get(id);
                return Ok(new ProductDto(product));
            }
            catch
            {
                return NotFound("Product not found");
            }

        }
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetProductBySlug(string slug)
        {
            try
            {
                var product = await _dataService.GetBySlug(slug);
                return Ok(new ProductDto(product));
            }
            catch
            {
                return NotFound("Product not found");
            }

        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreationDto productCreationDto)
        {
            if (productCreationDto == null)
                return BadRequest("Invalid product data");
            string imageUrl = "";
            if (productCreationDto.HeroImage != null)
            {
                var seoFileName = $"{productCreationDto.Name.ToUrlSlug()}-{Guid.NewGuid().ToString().Substring(0, 6)}";

                imageUrl = await _imageService.SaveImageAsWebP(
                    productCreationDto.HeroImage,
                    fileNameWithoutExtension: seoFileName,
                    folderName: "uploads",
                    maxWidth: 1200
                );
            }
            Product createdProduct = productCreationDto.ToModel();
            createdProduct.HeroImage = imageUrl;
            var product = await _dataService.Create(createdProduct);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, new ProductDto(product));
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductCreationDto productCreationDto)
        {
            try
            {
                var existingProduct = await _dataService.Get(id); // أو الطريقة المعتمدة عندك لجلب المنتج
                if (existingProduct == null)
                    return NotFound("Product not found");

                string? imageUrl = existingProduct.HeroImage;

                if (productCreationDto.HeroImage != null && productCreationDto.HeroImage.Length > 0)
                {
                    var seoFileName = $"{productCreationDto.Name.ToUrlSlug()}-{Guid.NewGuid().ToString().Substring(0, 6)}";

                    imageUrl = await _imageService.SaveImageAsWebP(
                        productCreationDto.HeroImage,
                        fileNameWithoutExtension: seoFileName,
                        folderName: "uploads",
                        maxWidth: 1200
                    );

                    if (!string.IsNullOrEmpty(existingProduct.HeroImage))
                    {
                        _imageService.DeleteImage(existingProduct.HeroImage);
                    }
                }

                Product product =productCreationDto.ToModel();
                product.Id = id;
                product.HeroImage = imageUrl; 

                var updated = await _dataService.Update(product);

                return Ok(new ProductDto(updated));
            }
            catch
            {
                return NotFound("Product not found");
            }
        }
    }
}
