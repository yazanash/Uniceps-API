using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.DTOs.ProductDtos.UserStepDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Products;
using Uniceps.Entityframework.Services.ProductServices;

namespace Uniceps.app.Controllers.ProductControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserStepController : ControllerBase
    {
        private readonly IProductRelatedDataService<UserStep> _stepService;

        public UserStepController(IProductRelatedDataService<UserStep> stepService)
        {
            _stepService = stepService;
        }
        [HttpGet("product/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            // ترتيب الخطوات حسب الـ StepNumber قبل الإرسال
            var steps = (await _stepService.GetAllByProductId(productId)).OrderBy(s => s.StepNumber);
            return Ok(steps.Select(x=>new UserStepDto(x)).ToList());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserStepCreationDro dto)
        {
            var step = dto.ToModel();
            await _stepService.Create(step);
            return Ok(new UserStepDto(step));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserStepCreationDro dto)
        {
            var step =dto.ToModel();
            step.Id = id;
            await _stepService.Update(step);
            return Ok("Updated");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _stepService.Delete(id);
            return Ok("Updated");
        }
    }
}
