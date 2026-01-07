using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using Uniceps.app.DTOs.ExerciseDtos;
using Uniceps.app.Services;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Services.ExerciseServices;

namespace Uniceps.app.Controllers.RoutineControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseDataService _dataService;
        private ILogger<ExerciseController> _logger;
        IMapperExtension<Exercise, ExerciseDto, ExerciseCreateDto> _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ExerciseImageService _exerciseImageService;
        public ExerciseController(ILogger<ExerciseController> logger,
            IMapperExtension<Exercise, ExerciseDto, ExerciseCreateDto> mapper, IWebHostEnvironment webHostEnvironment,
           IExerciseDataService dataService, ExerciseImageService exerciseImageService)
        {
            _logger = logger;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _dataService = dataService;
            _exerciseImageService = exerciseImageService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int id)
        {
            IEnumerable<Exercise> groups = await _dataService.GetAllById(id); 
            return Ok(groups.Select(x => _mapper.ToDto(x)).ToList());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ExerciseCreateDto exerciseDto)
        {
            if (exerciseDto == null || string.IsNullOrEmpty(exerciseDto.Name)||exerciseDto.Image==null)
                return BadRequest("بيانات التمرين غير مكتملة.");

            string fileName = exerciseDto.Name.Replace(" ", "_").ToLower() + ".webp";
            await _exerciseImageService.SaveImageAsWebP(exerciseDto.Image, fileName);

            // 2. تحويل الـ DTO لـ Entity
            var exercise = _mapper.FromCreationDto(exerciseDto);
            exercise.ImageUrl = fileName;

            // 3. نترك الخدمة تقرر (إضافة أو تحديث)
            var result = await _dataService.UpsertAsync(exercise);

            return Ok(result);
        }
        [HttpDelete("id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var exercise = await _dataService.Get(id);
            if (exercise != null && !string.IsNullOrEmpty(exercise.ImageUrl))
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "ExerciseImages", exercise.ImageUrl);
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }
            return Ok("Deleted successfully");
        }
        [HttpGet("ExerciseImages/{imageName}")]
        public IActionResult GetExerciseImage(string imageName)
        {
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "ExerciseImages", imageName);
            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound("Image not found.");
            }
            var image = System.IO.File.OpenRead(imagePath);
            Response.Headers.Append("Cache-Control", "public,max-age=2592000");


            return File(image, "image/webp");
        }
    }
}
