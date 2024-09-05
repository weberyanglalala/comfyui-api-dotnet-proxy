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

    public ComfyUIController(SimpleTextPromptService simpleTextPromptService,
        StableDiffusionPromptEnhancerService stableDiffusionPromptEnhancerService)
    {
        _simpleTextPromptService = simpleTextPromptService;
        _stableDiffusionPromptEnhancerService = stableDiffusionPromptEnhancerService;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrompt([FromBody] CreatePromptRequest request)
    {
        var enhancedPrompt = await _stableDiffusionPromptEnhancerService.CreateEnhancedPrompt(request.Prompt);
        var result = await _simpleTextPromptService.CreatePromptAsync(enhancedPrompt.PositivePrompt, enhancedPrompt.NegativePrompt);
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
}