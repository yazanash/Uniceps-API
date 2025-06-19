using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.Services.NotificationServices;
using Uniceps.Entityframework.Models.AuthenticationModels;

namespace Uniceps.app.Controllers.NotificationSystemControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationSender _notificationSender;

        public NotificationController(INotificationSender notificationSender)
        {
            _notificationSender = notificationSender;
        }
        [HttpPost("test")]
        public async Task<IActionResult> SendTestNotification([FromBody] NotificationDto notificationDto)
        {
            await _notificationSender.SendAsync(notificationDto.UserId!, notificationDto.Title!, notificationDto.Body!);
            return Ok(notificationDto);
        }
    }
    public class NotificationDto
    {
        public string? UserId { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
    }
}
