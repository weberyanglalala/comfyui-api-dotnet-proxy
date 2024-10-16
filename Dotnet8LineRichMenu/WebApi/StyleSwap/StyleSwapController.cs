using Dotnet8LineRichMenu.Models;
using Dotnet8LineRichMenu.Services;
using Dotnet8LineRichMenu.WebApi.StyleSwap.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet8LineRichMenu.WebApi.StyleSwap;

[Authorize]
[Route("api/[controller]/[action]")]
[ApiController]
public class StyleSwapController : ControllerBase
{
    private readonly SimpleTextPromptService _simpleTextPromptService;
    private readonly CloudinaryService _cloudinaryService;

    public StyleSwapController(SimpleTextPromptService simpleTextPromptService, CloudinaryService cloudinaryService)
    {
        _simpleTextPromptService = simpleTextPromptService;
        _cloudinaryService = cloudinaryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateStyleChangeImageByPrompt(
        [FromBody] CreateStyleChangeImageByPromptRequest request)
    {
        var promptId = await _simpleTextPromptService.CreateFluxStyleChangeImageWithStyleAndSeed(request.ImageName,
            request.Style, request.Seed, request.Width, request.Height);
        var response = new CreateStyleChangeImageByPromptResponse
        {
            PromptId = promptId, ImageName = request.ImageName, Style = request.Style, Seed = request.Seed,
            Width = request.Width, Height = request.Height
        };
        var apiResponse = new ApiResponse(response);
        return Ok(apiResponse);
    }

    public async Task<IActionResult> GetStyleChangeImageByPromptId([FromQuery] string promptId)
    {
        var status = await _simpleTextPromptService.GetPromptStatus(promptId);
        if (status)
        {
            var imageUrl = await _simpleTextPromptService.GetFluxStyleChangeImageByPromptId(promptId);
            var publicUrl = await _cloudinaryService.UploadSingleFileAsync(imageUrl);
            var apiResponse = new ApiResponse(new GetStyleChangeImageByPromptIdResponse()
            {
                IsSuccess = true,
                Message = "Prompt is Done.",
                PublicUrl = publicUrl
            });
            return Ok(apiResponse);
        }
        else
        {
            var apiResponse = new ApiResponse(new GetStyleChangeImageByPromptIdResponse()
            {
                IsSuccess = false,
                Message = "Prompt is not ready yet or is invalid scope."
            });
            return Ok(apiResponse);
        }
    }
}