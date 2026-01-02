using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.DTOs.ProductDtos.FAQDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Products;
using Uniceps.Entityframework.Services.ProductServices;

namespace Uniceps.app.Controllers.ProductControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class FAQController : ControllerBase
    {
        private readonly IProductRelatedDataService<FrequentlyAskedQuestion> _faqService;
        private readonly IMapperExtension<FrequentlyAskedQuestion, FrequentlyAskedQuestionDto, FrequentlyAskedQuestionCreationDto> _mapper;

        public FAQController(IProductRelatedDataService<FrequentlyAskedQuestion> faqService, IMapperExtension<FrequentlyAskedQuestion, FrequentlyAskedQuestionDto, FrequentlyAskedQuestionCreationDto> mapper)
        {
            _faqService = faqService;
            _mapper = mapper;
        }
        [HttpGet("product/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            var faqs = await _faqService.GetAllByProductId(productId);
            return Ok(faqs.Select(x=> _mapper.ToDto(x)).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FrequentlyAskedQuestionCreationDto dto)
        {
            var faq = _mapper.FromCreationDto(dto);
            var result = await _faqService.Create(faq);
            return Ok(_mapper.ToDto(result));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> update(int id,[FromBody] FrequentlyAskedQuestionCreationDto dto)
        {
            var faq = _mapper.FromCreationDto(dto);
            faq.Id = id;
            await _faqService.Update(faq);
            return Ok("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _faqService.Delete(id);
            return Ok("Deleted");
        }
    }
}
