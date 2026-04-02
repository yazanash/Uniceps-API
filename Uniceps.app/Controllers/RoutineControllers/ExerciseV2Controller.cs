using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Uniceps.app.DTOs.ExerciseDtos;
using Uniceps.app.Helpers;
using Uniceps.app.Services;
using Uniceps.Entityframework.Models.RoutineModelsV2;
using Uniceps.Entityframework.Services.ExerciseServices;

namespace Uniceps.app.Controllers.RoutineControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseV2Controller : ControllerBase
    {
        private readonly IExerciseV2DataService _dataService;
        private readonly ExerciseImageService _imageService;

        public ExerciseV2Controller(IExerciseV2DataService dataService, ExerciseImageService imageService)
        {
            _dataService = dataService;
            _imageService = imageService;
        }
        [HttpGet]
        public async Task<IActionResult> GetExercises([FromQuery] ExerciseFilterDto exerciseFilterDto)
        {
            bool isArabic = Request.GetLanguage() == "ar";
            ExerciseFilter exerciseFilter = exerciseFilterDto.ToModel();
            var exercises = await _dataService.GetAllExercisesAsync(exerciseFilter);
            var response = exercises.Select(x => new ExerciseV2Response(x, isArabic)).ToList();
            return Ok(response);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetEssentials()
        {
            bool isArabic = Request.GetLanguage() == "ar";
            EssentialsReponse essentialsReponse = new EssentialsReponse();
            var groups = await _dataService.GetMuscleGroupsAsync();
            var equipments = await _dataService.GetEquipmentssAsync();
            essentialsReponse.MuscleGroups = groups.Select(x => new MuscleGroupV2Response(x, isArabic)).ToList();
            essentialsReponse.Equipments = equipments.Select(x => new EquipmentsResponse(x, isArabic)).ToList();
            return Ok(essentialsReponse);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SyncEssentials([FromBody] EssentialsPayload payload)
        {
            try
            {

                List<MuscleGroupV2> groups = payload.MuscleGroups
             .Select(x => new MuscleGroupV2
             {
                 Code = x.Code,
                 NameAr = x.NameAr,
                 NameEn = x.NameEn
             }).ToList();
                List<Equipment> equipments = payload.Equipments.Select(x => new Equipment { Code = x.Code, NameAr = x.NameAr, NameEn = x.NameEn }).ToList();
                List<MuscleHead> heads = payload.Heads.Select(x => new MuscleHead { Code = x.Code, NameAr = x.NameAr, NameEn = x.NameEn, MuscleGroupCode = x.MuscleGroupCode }).ToList();
                await _dataService.SyncMuscleGroupsAsync(groups);
                await _dataService.SyncEquipmentsAsync(equipments);
                await _dataService.SyncMuscleHeads(heads);
                return Ok("Muscles and equipments uploaded successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadExercise([FromForm] ExerciseUploadRequest request)
        {
            try
            {
                string imageUrl = "";

                if (request.ImageFile != null)
                {
                    imageUrl = await _imageService.SaveImageAsWebP(request.ImageFile, request.ExerciseId, "ExercisesV2");
                }
                var exercise = new ExerciseV2
                {
                    ExerciseId = request.ExerciseId,
                    NameEn = request.NameEn,
                    NameAr = request.NameAr,
                    MuscleGroupCode = request.MuscleGroupCode,
                    MuscleHeadCode = request.MuscleHeadCode,
                    EquipmentCode = request.EquipmentCode,
                    Mechanism = GetExerciseMechanisim(request.Mechanism),
                    ImageUrl = imageUrl,
                    MuscleAux1 = request.MuscleAux1,
                    MuscleAux2 = request.MuscleAux2,
                    MuscleAux3 = request.MuscleAux3,
                    LastUpdated = DateTime.UtcNow,
                    Implementation = request.Implementation,
                    Version = request.Version,
                };

                await _dataService.SyncExercisesAsync(new List<ExerciseV2> { exercise });

                return Ok(new { Message = $" Exercise {exercise.NameEn} Uploaded sucessfully", Url = imageUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private ExerciseMechanism GetExerciseMechanisim(string uniBi)
        {
            foreach (var item in Enum.GetValues(typeof(ExerciseMechanism)))
            {
                if (uniBi.Trim().ToLower().Equals(item.ToString()?.Trim().ToLower()))
                {
                    return (ExerciseMechanism)item;
                }
            }
            return ExerciseMechanism.Bi;
        }
        [HttpGet("get-image/{exerciseId}")]
        public async Task<IActionResult> GetExerciseImage(string exerciseId)
        {
            var relativePath = await _dataService.GetExerciseImage(exerciseId);
            if (string.IsNullOrEmpty(relativePath)) return NotFound("Image path not defined.");
            var filePath = _imageService.GetImagePath(relativePath);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Image file not found on server.");
            }
            var imageStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(imageStream, "image/webp");
        }
    }
}
