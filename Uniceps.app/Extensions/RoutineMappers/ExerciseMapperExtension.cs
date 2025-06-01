using Uniceps.app.DTOs.ExerciseDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.Extensions.RoutineMappers
{
    public class ExerciseMapperExtension : IMapperExtension<Exercise, ExerciseDto, ExerciseCreateDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExerciseMapperExtension(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Exercise FromCreationDto(ExerciseCreateDto data)
        {
            return new Exercise()
            {
                MuscleGroupId = data.MuscleGroupId,
                Name = data.Name,
            };
        }

        public ExerciseDto ToDto(Exercise data)
        {
            var request = _httpContextAccessor.HttpContext!.Request;
            string? imageUrl = string.IsNullOrEmpty(data.ImageUrl)
                ? null
                : $"{request.Scheme}://{request.Host}/api/Exercise/ExerciseImages/{data.ImageUrl}";
            return new ExerciseDto()
            {
                Id = data.Id,
                Name = data.Name,
                MuscleGroupId = data.MuscleGroupId,
                ImageUrl = imageUrl
            };
        }
    }
}
