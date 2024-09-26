using Dotnet8LineRichMenu.Models;
using Dotnet8LineRichMenu.Services;
using Dotnet8LineRichMenu.WebApi.FurnitureDesign.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet8LineRichMenu.WebApi.FurnitureDesign;

[ApiController]
[Route("api/[controller]/[action]")]
public class FurnitureDesignController : ControllerBase
{
    private readonly SimpleTextPromptService _simpleTextPromptService;
    private readonly CloudinaryService _cloudinaryService;

    public FurnitureDesignController(SimpleTextPromptService simpleTextPromptService,
        CloudinaryService cloudinaryService)
    {
        _simpleTextPromptService = simpleTextPromptService;
        _cloudinaryService = cloudinaryService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFurnitureDesign([FromBody] CreateFurnitureDesignRequest request)
    {
        var promptId = await _simpleTextPromptService.CreateFurnitureDesign(request.ImagePath, request.Style, request.Seed);
        var response = new CreateFurnitureDesignResponse
        {
            PromptId = promptId,
            Style = request.Style,
            Seed = request.Seed
        };
        var apiResponse = new ApiResponse(response);
        return Ok(apiResponse);
    }
    
    public async Task<IActionResult> GetFurnitureDesignByPromptId([FromQuery] string promptId)
    {
        var status = await _simpleTextPromptService.GetPromptStatus(promptId);
        if (status)
        {
            var imageUrl = await _simpleTextPromptService.GetFurnitureDesignImageByPromptId(promptId);
            var publicUrl = await _cloudinaryService.UploadSingleFileAsync(imageUrl);
            var apiResponse = new ApiResponse(new GetFurnitureDesignImageResponse()
            {
                IsSuccess = true,
                Message = "Prompt is Done.",
                PublicUrl = publicUrl
            });
            return Ok(apiResponse);
        }
        else
        {
            var apiResponse = new ApiResponse(new GetFurnitureDesignImageResponse()
            {
                IsSuccess = false,
                Message = "Prompt is not ready yet or is invalid scope."
            });
            return Ok(apiResponse);
        }
    }
}