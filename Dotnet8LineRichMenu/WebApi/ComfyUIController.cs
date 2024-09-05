using Dotnet8LineRichMenu.Models.ComfyUIApi.Prompt;
using Dotnet8LineRichMenu.Models.Dtos.ComfyUI;
using Dotnet8LineRichMenu.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet8LineRichMenu.WebApi.ComfyUI;

[ApiController]
[Route("api/[controller]/[action]")]
public class ComfyUIController : ControllerBase
{
    private readonly SimpleTextPromptService _simpleTextPromptService;
    private readonly StableDiffusionPromptEnhancerService _stableDiffusionPromptEnhancerService;
    private readonly CloudinaryService _cloudinaryService;

    public ComfyUIController(SimpleTextPromptService simpleTextPromptService,
        StableDiffusionPromptEnhancerService stableDiffusionPromptEnhancerService,
        CloudinaryService cloudinaryService)
    {
        _simpleTextPromptService = simpleTextPromptService;
        _stableDiffusionPromptEnhancerService = stableDiffusionPromptEnhancerService;
        _cloudinaryService = cloudinaryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrompt([FromBody] CreatePromptRequest request)
    {
        var enhancedPrompt = await _stableDiffusionPromptEnhancerService.CreateEnhancedPrompt(request.Prompt);
        var result =
            await _simpleTextPromptService.CreatePromptAsync(enhancedPrompt.PositivePrompt,
                enhancedPrompt.NegativePrompt);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetImageByPromptId([FromQuery] string promptId)
    {
        var result = await _simpleTextPromptService.GetImageByPromptId(promptId);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetPromptStatus([FromQuery] string promptId)
    {
        var result = await _simpleTextPromptService.GetPromptStatus(promptId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEnhancePrompt([FromBody] CreateEnhancePrompt request)
    {
        var result = await _stableDiffusionPromptEnhancerService.CreateEnhancedPrompt(request.Prompt);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateImageByPrompt([FromBody] CreateImageByPrompt request)
    {
        var promptObject =
            await _stableDiffusionPromptEnhancerService.CreateEnhancedPrompt(request.Prompt);
        var promptId =
            await _simpleTextPromptService.CreatePromptAsync(promptObject.PositivePrompt,
                promptObject.NegativePrompt);
        bool promptStatus = false;
        promptStatus = await _simpleTextPromptService.CheckPromptStatusWithRetry(promptId);
        if (!promptStatus)
        {
            return BadRequest("系統忙碌中，請稍後再試。");
        }
        var imageUrl = await _simpleTextPromptService.GetImageByPromptId(promptId);
        var publicUrl = await _cloudinaryService.UploadSingleFileAsync(imageUrl);
        return Ok(publicUrl);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateImageByDetailPrompt([FromBody] CreateImageByDetailPrompt request)
    {
        var promptId =
            await _simpleTextPromptService.CreatePromptAsync(request.PositivePrompt,
                request.NegativePrompt);
        bool promptStatus = false;
        promptStatus = await _simpleTextPromptService.CheckPromptStatusWithRetry(promptId);
        if (!promptStatus)
        {
            return BadRequest("系統忙碌中，請稍後再試。");
        }
        var imageUrl = await _simpleTextPromptService.GetImageByPromptId(promptId);
        var publicUrl = await _cloudinaryService.UploadSingleFileAsync(imageUrl);
        return Ok(publicUrl);
    }
}