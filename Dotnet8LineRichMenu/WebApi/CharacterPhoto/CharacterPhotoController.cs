using Dotnet8LineRichMenu.Models;
using Dotnet8LineRichMenu.Services;
using Dotnet8LineRichMenu.WebApi.CharacterPhoto.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet8LineRichMenu.WebApi.CharacterPhoto;

[Route("api/[controller]/[action]")]
[ApiController]
public class CharacterPhotoController : ControllerBase
{
    private readonly SimpleTextPromptService _simpleTextPromptService;
    private readonly CloudinaryService _cloudinaryService;

    public CharacterPhotoController(SimpleTextPromptService simpleTextPromptService, CloudinaryService cloudinaryService)
    {
        _simpleTextPromptService = simpleTextPromptService;
        _cloudinaryService = cloudinaryService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateCharacterPhoto([FromBody] CreateCharacterPhotoRequest request)
    {
        var promptId = 
            await _simpleTextPromptService.CreateCharacterPhoto(
                request.ImagePath, request.Style, request.Seed, request.Count, request.Width, request.Height);
        var response = new CreateCharacterPhotoResponse
        {
            PromptId = promptId,
            Style = request.Style,
            Seed = request.Seed,
            Count = request.Count,
            Width = request.Width,
            Height = request.Height
        };
        var apiResponse = new ApiResponse(response);
        return Ok(apiResponse);
    }
    
    public async Task<IActionResult> GetCharacterPhotosByPromptId([FromQuery] string promptId)
    {
        var status = await _simpleTextPromptService.GetPromptStatus(promptId);
        List<string> imageUrls = new List<string>();
        if (status)
        {
            var images = await _simpleTextPromptService.GetCharacterPhotosByPromptId(promptId);
            foreach (var image in images)
            {
                var publicUrl = await _cloudinaryService.UploadSingleFileAsync(image);
                imageUrls.Add(publicUrl);
            }
            var apiResponse = new ApiResponse(new GetCharacterPhotosByPromptIdResponse()
            {
                PublicUrls = imageUrls
            });
            return Ok(apiResponse);
        }
        else
        {
            var apiResponse = new ApiResponse()
            {
                IsSuccess = false,
                Message = "Prompt is not ready yet or is invalid scope."
            };
            return Ok(apiResponse);
        }
    }
}