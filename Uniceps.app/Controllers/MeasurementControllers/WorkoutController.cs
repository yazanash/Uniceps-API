using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Telegram.Bot.Types;
using Uniceps.app.DTOs.MeasurementDtos;
using Uniceps.app.Helpers;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Models.Measurements;
using Uniceps.Entityframework.Services;
using Uniceps.Entityframework.Services.MeasurementServices;

namespace Uniceps.app.Controllers.MeasurementControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IWorkoutSessionService _dataService;
        private readonly IMapperExtension<WorkoutSession, WorkoutSessionDto, WorkoutSessionCreationDto> _mapperExtension;
        private readonly INotificationDataService _notificationDataService;
        public WorkoutController(IWorkoutSessionService dataService, IMapperExtension<WorkoutSession, WorkoutSessionDto, WorkoutSessionCreationDto> mapperExtension, INotificationDataService notificationDataService)
        {
            _dataService = dataService;
            _mapperExtension = mapperExtension;
            _notificationDataService = notificationDataService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
           
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            IEnumerable<WorkoutSession> players = await _dataService.GetAllByUser(userId);
            return Ok(players.Select(x => _mapperExtension.ToDto(x)).ToList());
        }
        [HttpPost]
        public async Task<IActionResult> Create(WorkoutSessionCreationDto bodyMeasurementCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
         
            if (bodyMeasurementCreationDto == null)
                return BadRequest("Workout session data is missing.");

            WorkoutSession bodyMeasurement = _mapperExtension.FromCreationDto(bodyMeasurementCreationDto);
            bodyMeasurement.UserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var totalMinutes = bodyMeasurement.FinishedAt?.Subtract(bodyMeasurement.CreatedAt).TotalMinutes ?? 0;
             string Length = Math.Ceiling(totalMinutes).ToString("0");
            //await _notificationDataService.CreateAsync(new Notification
            //{
            //    UserId = bodyMeasurement.UserId,
            //    Title = $"كيفك يابطل",
            //    Body = "تمرينك المعتاد بيبدأ بعد ساعة، جاهز؟",
            //    ScheduledTime = DateTime.UtcNow.AddHours(22)
            //});
            //if (!string.IsNullOrEmpty(Length))
            //{
            //    await _notificationDataService.CreateAsync(new Notification
            //    {
            //        UserId = bodyMeasurement.UserId,
            //        Title = $"حماستك بالاداء رهيبة",
            //        Body = $"{Length} دقائق في التمرين رائعة",
            //        ScheduledTime = DateTime.UtcNow.AddMinutes(10)
            //    });
            //}
           
            var result = await _dataService.UpsertAsync(bodyMeasurement);
            return Ok(new WorkoutSessionIdDto() { Id= result.Id});
        }
    }
}
