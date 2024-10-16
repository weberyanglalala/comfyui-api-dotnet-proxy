using Dotnet8LineRichMenu.Models;
using Dotnet8LineRichMenu.Services;
using Dotnet8LineRichMenu.WebApi.FluxOptimize.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet8LineRichMenu.WebApi.FluxOptimize;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class FluxOptimizeController : ControllerBase
{
    private readonly SimpleTextPromptService _simpleTextPromptService;

    public FluxOptimizeController(SimpleTextPromptService simpleTextPromptService)
    {
        _simpleTextPromptService = simpleTextPromptService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateWithSeedAndPromptAndSize(
        [FromBody] CreateBySeedPromptSizeRequest request)
    {
        var promptId =
            await _simpleTextPromptService.CreateFluxPromptWithSeedAndCustomPromptAndSize(
                request.Prompt, request.Seed, request.Width, request.Height
            );
        var apiResponse = new ApiResponse(new CreateBySeedPromptSizeResponse
        {
            PromptId = promptId,
            Prompt = request.Prompt,
            Seed = request.Seed,
            Width = request.Width,
            Height = request.Height
        });
        return Ok(apiResponse);
    }
}