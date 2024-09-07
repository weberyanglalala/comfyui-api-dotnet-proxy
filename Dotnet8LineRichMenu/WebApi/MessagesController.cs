using Dotnet8LineRichMenu.Models.Settings;
using Dotnet8LineRichMenu.Services;
using isRock.LineBot;

namespace Dotnet8LineRichMenu.WebApi;

using Microsoft.AspNetCore.Mvc;

public class MessagesController : LineWebHookControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly LineMessagingApiSettings _lineMessagingApiSettings;
    private readonly SimpleTextPromptService _simpleTextPromptService;
    private readonly StableDiffusionPromptEnhancerService _stableDiffusionPromptEnhancerService;
    private readonly CloudinaryService _cloudinaryService;
    private readonly string _adminUserId;
    private readonly string _domain;

    public MessagesController(
        LineMessagingApiSettings lineMessagingApiSettings, IConfiguration configuration,
        SimpleTextPromptService simpleTextPromptService,
        StableDiffusionPromptEnhancerService stableDiffusionPromptEnhancerService, CloudinaryService cloudinaryService)
    {
        _lineMessagingApiSettings = lineMessagingApiSettings;
        _configuration = configuration;
        _simpleTextPromptService = simpleTextPromptService;
        _stableDiffusionPromptEnhancerService = stableDiffusionPromptEnhancerService;
        _cloudinaryService = cloudinaryService;
        _adminUserId = lineMessagingApiSettings.UserId;
        _domain = configuration["Domain"];
        ChannelAccessToken = lineMessagingApiSettings.ChannelAccessToken;
    }

    [Route("api/LineBotChatWebHook")]
    [HttpPost]
    public async Task<IActionResult> LineBotChatWebHook()
    {
        try
        {
            if (IsLineVerify()) return Ok();
            foreach (var lineEvent in ReceivedMessage.events)
            {
                DisplayLoadingAnimation(lineEvent.source.userId, 30);
                var promptId =
                    await _simpleTextPromptService.CreateFluxOptimizedAsync(lineEvent.message.text);
                bool promptStatus = false;
                promptStatus = await _simpleTextPromptService.CheckPromptStatusWithRetry(promptId);
                if (!promptStatus)
                {
                    PushMessage(lineEvent.source.userId, "系統忙碌中，請稍後再試。");
                    return Ok();
                }

                var imageUrl = await _simpleTextPromptService.GetImageByPromptId(promptId);
                var publicUrl = await _cloudinaryService.UploadSingleFileAsync(imageUrl);
                Console.WriteLine(publicUrl);
                //回覆訊息
                PushMessage(lineEvent.source.userId, new Uri(publicUrl.ToString()));
            }

            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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

    private void SetupRichMenu()
    {
        // 建立Rich Menu結構
        var item = new isRock.LineBot.RichMenu.RichMenuItem
        {
            name = "六宮格選單",
            chatBarText = "選單"
        };

        // 設定六個按鈕區塊
        var buttons = new List<isRock.LineBot.RichMenu.Area>
        {
            new isRock.LineBot.RichMenu.Area
            {
                bounds = new isRock.LineBot.RichMenu.Bounds { x = 0, y = 0, width = 833, height = 843 },
                action = new isRock.LineBot.UriAction()
                {
                    label = "Social Media",
                    uri = new Uri($"{_domain}/Home/SocialMedia")
                }
            },
            new isRock.LineBot.RichMenu.Area
            {
                bounds = new isRock.LineBot.RichMenu.Bounds { x = 833, y = 0, width = 833, height = 843 },
                action = new isRock.LineBot.UriAction()
                {
                    label = "Social Media UrL",
                    uri = new Uri($"{_domain}/Home/SocialMediaUrl")
                }
            },
            new isRock.LineBot.RichMenu.Area
            {
                bounds = new isRock.LineBot.RichMenu.Bounds { x = 1666, y = 0, width = 834, height = 843 },
                action = new isRock.LineBot.UriAction()
                {
                    label = "Markdown",
                    uri = new Uri($"{_domain}/Home/Markdown")
                }
            },
            new isRock.LineBot.RichMenu.Area
            {
                bounds = new isRock.LineBot.RichMenu.Bounds { x = 0, y = 843, width = 833, height = 843 },
                action = new isRock.LineBot.UriAction()
                {
                    label = "Marketing",
                    uri = new Uri($"{_domain}/Home/Marketing")
                }
            },
            new isRock.LineBot.RichMenu.Area
            {
                bounds = new isRock.LineBot.RichMenu.Bounds { x = 833, y = 843, width = 833, height = 843 },
                action = new isRock.LineBot.UriAction()
                {
                    label = "News",
                    uri = new Uri($"{_domain}/Home/News")
                }
            },
            new isRock.LineBot.RichMenu.Area
            {
                bounds = new isRock.LineBot.RichMenu.Bounds { x = 1666, y = 843, width = 834, height = 843 },
                action = new isRock.LineBot.UriAction()
                {
                    label = "王道銀行小幫手",
                    uri = new Uri($"{_domain}/Home/Bank")
                }
            }
        };

        // 加入按鈕區塊
        foreach (var button in buttons)
        {
            item.areas.Add(button);
        }

        // 建立Rich Menu並綁定圖像
        var menu = isRock.LineBot.Utility.CreateRichMenu(
            item,
            UploadRichMenuImageFromWwwroot("rich_menu_image_with_new_icons.png"),
            ChannelAccessToken
        );

        // 設定預設Rich Menu
        isRock.LineBot.Utility.SetDefaultRichMenu(menu.richMenuId, ChannelAccessToken);
        Console.WriteLine(menu.richMenuId);
        Console.WriteLine("Rich Menu Setup Done.");
    }

    public byte[] UploadRichMenuImageFromWwwroot(string filePath)
    {
        // Construct the full path to the file in the wwwroot folder
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);
        // Read the file into a byte array
        byte[] pngImage = System.IO.File.ReadAllBytes(fullPath);
        return pngImage;
    }
}