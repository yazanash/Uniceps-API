using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.NutritionDtos;
using Uniceps.app.Helpers;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Measurements;
using Uniceps.Entityframework.Models.NutritionSystem;
using Uniceps.Entityframework.Services.NutritionServices;

namespace Uniceps.app.Controllers.NutritionControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        public IIngredientDataService _ingredientDataService;

        public IngredientsController(IIngredientDataService ingredientDataService)
        {
            _ingredientDataService = ingredientDataService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(DateTime? lastSync)
        {
            //try
            //{
                bool isArabic = Request.GetLanguage() == "ar";
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                IEnumerable<Ingredient> ingredients = await _ingredientDataService.GetAll(userId, lastSync);
                return Ok(ingredients.Select(x => new IngredientResponse(x, isArabic)).ToList());
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}
        }
        [HttpPost("custom")]
        public async Task<IActionResult> CreateIngredient(IngredientRequest ingredientRequest)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            bool isArabic = Request.GetLanguage() == "ar";
            Ingredient ingredient = ingredientRequest.ToModel();
            ingredient.UserId = userId;
            ingredient.IsUserGenerated = true;
            Ingredient createdIngredients = await _ingredientDataService.UpsertAsync(ingredient);
            return Ok(new IngredientResponse(createdIngredients, isArabic));
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateIngredientByAdmin(IngredientRequest ingredientRequest)
        {
            Ingredient ingredient = ingredientRequest.ToModel();
            ingredient.IsVerified = true;
            bool isArabic = Request.GetLanguage() == "ar";
            Ingredient createdIngredients = await _ingredientDataService.UpsertAsync(ingredient);
            return Ok(new IngredientResponse(createdIngredients, isArabic));
        }
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            IEnumerable<IngredientCategory> ingredientCategories = await _ingredientDataService.GetAllCategories();
            return Ok(ingredientCategories.Select(x => new IngredientCategoryResponse(x)).ToList());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("categories")]
        public async Task<IActionResult> CreateIngredientCategory(IngredientCategoryRequest ingredientCategoryRequest)
        {
            IngredientCategory ingredient = ingredientCategoryRequest.ToModel();
            IngredientCategory createdIngredientCategory = await _ingredientDataService.CreateCategory(ingredient);
            return Ok(new IngredientCategoryResponse(createdIngredientCategory));
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateIngredientCategory(int id, [FromBody] IngredientCategoryRequest ingredientCategoryRequest)
        {
            IngredientCategory ingredient = ingredientCategoryRequest.ToModel();
            ingredient.Id = id;
            IngredientCategory createdIngredientCategory = await _ingredientDataService.CreateCategory(ingredient);
            return Ok(new IngredientCategoryResponse(createdIngredientCategory));
        }
    }
}
