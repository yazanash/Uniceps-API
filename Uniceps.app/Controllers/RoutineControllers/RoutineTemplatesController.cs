using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Uniceps.app.DTOs.RoutineDtos;
using Uniceps.app.Helpers;
using Uniceps.app.Helpers.UniFileDtos;
using Uniceps.app.Services;
using Uniceps.Entityframework.Models.RoutineModelsV2;
using Uniceps.Entityframework.Services.RoutineServices;

namespace Uniceps.app.Controllers.RoutineControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutineTemplatesController : ControllerBase
    {
        private readonly IRoutineTemplateDataService _routineTemplateDataService;
        private readonly IUniFileParserService _uniFileParserService;
        public RoutineTemplatesController(IRoutineTemplateDataService routineTemplateDataService, IUniFileParserService uniFileParserService)
        {
            _routineTemplateDataService = routineTemplateDataService;
            _uniFileParserService = uniFileParserService;
        }
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllForAdmin()
        {
            try
            {
                var templates = await _routineTemplateDataService.GetAllByQuery(null,null);
                return Ok(templates.Select(x => new RoutineTemplateResponse(x)));
            }
            catch (Exception)
            {
                return NotFound("القالب غير موجود.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(TargetGender? targetGender)
        {
            try
            {
                TargetLanguage? targetLanguage;
                bool isArabic = Request.GetLanguage() == "ar";
                if (isArabic) { targetLanguage = TargetLanguage.Arabic; }
                else { targetLanguage = TargetLanguage.English; }

                var templates = await _routineTemplateDataService.GetAllByQuery(targetGender, targetLanguage);
                return Ok(templates.Select(x => new RoutineTemplateResponse(x)));
            }
            catch (Exception)
            {
                return NotFound("القالب غير موجود.");
            }
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadTemplate([FromForm] UploadRoutineTemplateRequest request)
        {
            try
            {
                if (request.File == null || request.File.Length == 0)
                    return BadRequest("ملف القالب مطلوب.");
                var extension = Path.GetExtension(request.File.FileName).ToLowerInvariant();
                if (extension != ".unx")
                {
                    return BadRequest("نوع الملف غير مدعوم، يجب أن يكون الملف بصيغة .unx حصراً.");
                }

                try
                {
                    bool isArabic = Request.GetLanguage() == "ar";
                    using var stream = request.File.OpenReadStream();

                    var (uniFile, rawDataJson) = await _uniFileParserService.ParseRoutineUniFileAsync(stream);

                    RoutineExportDto routineData = uniFile.Data!;

                    int calculatedDaysCount = routineData.Days?.Count ?? 0;
                    int totalExercisesCount = routineData.Days?.Sum(d => d.Items.Count) ?? 0;

                    var templateEntity = new RoutineTemplate
                    {
                        Id = Guid.NewGuid(),
                        Title = request.Title ?? routineData.RoutineName,
                        Description = request.Description ?? routineData.Description,
                        TargetGender = request.TargetGender,
                        Level = request.Level,
                        DaysCount = calculatedDaysCount,
                        IsActive = true,
                        PayloadJson = rawDataJson,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        TargetLanguage = request.TargetLanguage
                    };

                    var createdTemplate = await _routineTemplateDataService.CreateAsync(templateEntity);

                    return CreatedAtAction(nameof(UploadTemplate), new RoutineTemplateResponse(templateEntity));
                }
                catch (InvalidDataException ex)
                {
                    return BadRequest(ex.Message);
                }
                catch (JsonException)
                {
                    return BadRequest("بنية الملف غير صالحة ولا تطابق صيغة JSON. ");
                }
                catch (Exception ex)
                {
                    return StatusCode(200, $"حدث خطأ أثناء معالجة الملف: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(200, $"حدث خطأ أثناء معالجة الملف: {ex.Message}");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                bool isArabic = Request.GetLanguage() == "ar";
                var template = await _routineTemplateDataService.Get(id);
                var unx = _uniFileParserService.ParseRoutineJson(template.PayloadJson);
                var parsedFile = unx.ParsedFile;
                var options = new JsonSerializerOptions { PropertyNamingPolicy = null };
                return Content(JsonSerializer.Serialize(parsedFile, options), "application/json");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] UploadRoutineTemplateRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("ملف القالب مطلوب.");
            var extension = Path.GetExtension(request.File.FileName).ToLowerInvariant();
            if (extension != ".unx")
            {
                return BadRequest("نوع الملف غير مدعوم، يجب أن يكون الملف بصيغة .unx حصراً.");
            }

            try
            {
                bool isArabic = Request.GetLanguage() == "ar";
                Guid Id = Guid.Parse(id);
                using var stream = request.File.OpenReadStream();

                var (uniFile, rawDataJson) = await _uniFileParserService.ParseRoutineUniFileAsync(stream);

                RoutineExportDto routineData = uniFile.Data!;

                int calculatedDaysCount = routineData.Days?.Count ?? 0;
                int totalExercisesCount = routineData.Days?.Sum(d => d.Items.Count) ?? 0;
                var templateEntity = await _routineTemplateDataService.Get(Id);
                templateEntity.Title = request.Title ?? routineData.RoutineName;
                templateEntity.Description = request.Description ?? routineData.Description;
                templateEntity.TargetGender = request.TargetGender;
                templateEntity.Level = request.Level;
                templateEntity.DaysCount = calculatedDaysCount;
                templateEntity.IsActive = true;
                templateEntity.PayloadJson = rawDataJson;
                templateEntity.UpdatedAt = DateTime.UtcNow;
                templateEntity.TargetLanguage = request.TargetLanguage;

                var createdTemplate = await _routineTemplateDataService.UpdateAsync(templateEntity);

                return Ok(new RoutineTemplateResponse(templateEntity));
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (JsonException)
            {
                return BadRequest("بنية الملف غير صالحة ولا تطابق صيغة JSON.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"حدث خطأ أثناء معالجة الملف: {ex.Message}");
            }
        }
    }
}
