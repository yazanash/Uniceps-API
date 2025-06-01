using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.DTOs.ExerciseDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Commands;
using Uniceps.Entityframework.Commands.ExerciseCommands;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Queries.ExerciseQueries;

namespace Uniceps.app.Controllers.RoutineControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private ILogger<ExerciseController> _logger;
        IMapperExtension<Exercise, ExerciseDto, ExerciseCreateDto> _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ExerciseController(IMediator mediator, ILogger<ExerciseController> logger, IMapperExtension<Exercise, ExerciseDto, ExerciseCreateDto> mapper, IWebHostEnvironment webHostEnvironment)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int id)
        {
            IEnumerable<Exercise> groups = await _mediator.Send(new GetAllExercisesQuery(id));
            return Ok(groups.Select(x => _mapper.ToDto(x)).ToList());
        }
        [HttpPost]
        public async Task<IActionResult> Create(ExerciseCreateDto exerciseDto)
        {
            if (exerciseDto == null)
                return BadRequest("Exercise data is missing.");

            // Convert DTO to domain model
            Exercise exercise = _mapper.FromCreationDto(exerciseDto);

            if (exerciseDto.Image != null && exerciseDto.Image.Length > 0)
            {
                // Save the file as before and obtain a unique file name.
                string uploadsFolder = Path.Combine(_webHostEnvironment.ContentRootPath, "ExerciseImages");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + exerciseDto.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await exerciseDto.Image.CopyToAsync(fileStream);
                }
                exercise.ImageUrl = uniqueFileName;
            }

            var result = await _mediator.Send(new AddExerciseCommand(exercise));
            _logger.LogInformation("Created Successfully");
            return Ok(_mapper.ToDto(exercise));
        }
        [HttpPut]
        public async Task<IActionResult> Update(ExerciseCreateDto exerciseDto)
        {
            Exercise exercise = _mapper.FromCreationDto(exerciseDto);
            var result = await _mediator.Send(new UpdateExerciseCommand(exercise));
            return Ok("Updated successfully");
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteExerciseCommand(id));
            return Ok("Deleted successfully");
        }
        [HttpGet("ExerciseImages/{imageName}")]
        public IActionResult GetExerciseImage(string imageName)
        {
            var imagePath = Path.Combine(_webHostEnvironment.ContentRootPath, "ExerciseImages", imageName);
            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound("Image not found.");
            }
            var image = System.IO.File.OpenRead(imagePath);
            return File(image, "image/jpeg"); // Adjust the MIME type as needed
        }
    }
}
