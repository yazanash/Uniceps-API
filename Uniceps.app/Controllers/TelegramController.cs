using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Uniceps.app.Services;

namespace Uniceps.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        private readonly TelegramBotService _botService;

        public TelegramController(TelegramBotService botService)
        {
            _botService = botService;
        }

        [HttpPost("update")]
        public async Task<IActionResult> ReceiveUpdate([FromBody] Update update)
        {
            await _botService.HandleUpdate(update);
            return Ok();
        }
    }
}
