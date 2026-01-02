using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.DTOs.ProductDtos;
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
        private readonly IMapperExtension<Product, ProductDto, ProductCreationDto> _mapperExtension;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductDataService dataService, IMapperExtension<Product, ProductDto, ProductCreationDto> mapperExtension, IWebHostEnvironment webHostEnvironment)
        {
            _dataService = dataService;
            _mapperExtension = mapperExtension;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _dataService.GetAll();
            return Ok(products.Select(x => _mapperExtension.ToDto(x)));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _dataService.Get(id);
                return Ok(_mapperExtension.ToDto(product));
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
                // تحديد مسار المجلد (wwwroot/uploads)
                var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                // إنشاء اسم فريد للملف
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productCreationDto.HeroImage.FileName);
                var filePath = Path.Combine(uploads, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productCreationDto.HeroImage.CopyToAsync(stream);
                }

                imageUrl = $"/uploads/{fileName}"; // الرابط الذي سيخزن في الداتابيز
            }
            Product createdProduct = _mapperExtension.FromCreationDto(productCreationDto);
            createdProduct.HeroImage = imageUrl;
            var product = await _dataService.Create(createdProduct);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, _mapperExtension.ToDto(product));
        }

        // تعديل منتج
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductCreationDto productCreationDto)
        {
            try
            {
                string imageUrl = "";
                if (productCreationDto.HeroImage != null)
                {
                    // تحديد مسار المجلد (wwwroot/uploads)
                    var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                    // إنشاء اسم فريد للملف
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productCreationDto.HeroImage.FileName);
                    var filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await productCreationDto.HeroImage.CopyToAsync(stream);
                    }

                    imageUrl = $"/uploads/{fileName}"; // الرابط الذي سيخزن في الداتابيز
                }
                Product product = _mapperExtension.FromCreationDto(productCreationDto);
                if(!string.IsNullOrEmpty(imageUrl))
                product.HeroImage = imageUrl;
                product.Id = id;
                var updated = await _dataService.Update(product);
                return Ok(_mapperExtension.ToDto(product));
            }
            catch
            {
                return NotFound("Product not found");
            }
        }
    }
}
