using Dotnet8LineRichMenu.Models.Settings;
using Dotnet8LineRichMenu.Services.DifyLineChat;
using isRock.LineBot;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Dotnet8LineRichMenu.WebApi;

public class DifyLineChatController : LineWebHookControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly string _adminUserId;
    private readonly DifyLineChatService _difyLineChatService;

    public DifyLineChatController(IConfiguration configuration, IOptions<DifyLineChatSettings> difyLineChatSettings, DifyLineChatService difyLineChatService)
    {
        _configuration = configuration;
        _difyLineChatService = difyLineChatService;
        _adminUserId = difyLineChatSettings.Value.UserId;
        ChannelAccessToken = difyLineChatSettings.Value.ChannelAccessToken;
    }

    [Route("api/hikaru/LineBotChatWebHook")]
    [HttpPost]
    public async Task<IActionResult> LineBotChatWebHook()
    {
        try
        {
            if (IsLineVerify()) return Ok();
            foreach (var lineEvent in ReceivedMessage.events)
            {
                DisplayLoadingAnimation(lineEvent.source.userId, 30);
                //回覆訊息
                if (lineEvent.type == "message")
                {
                    var message = lineEvent.message.text;
                    var userId = lineEvent.source.userId;
                    var user = GetUserInfo(userId);
                    var response = await _difyLineChatService.HandleLineChat(lineEvent.source.userId, user.displayName, message);
                    PushMessage(lineEvent.source.userId, response);
                }
            }

            return Ok();
        }
        catch (Exception ex)
        {
            PushMessage(_adminUserId, ex.Message);
            return Ok();
        }
    }

    private bool IsLineVerify()
    {
        return ReceivedMessage.events == null || ReceivedMessage.events.Count() <= 0 ||
               ReceivedMessage.events.FirstOrDefault().replyToken == "00000000000000000000000000000000";
    }

    private void DisplayLoadingAnimation(string chatId, int delay)
    {
        var bot = new Bot(ChannelAccessToken);
        bot.DisplayLoadingAnimation(chatId, delay);
    }
}