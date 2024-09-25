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
        if (result)
        {
            return Ok(new { IsSuccess = true, Message = "Prompt is Done." });
        }
        else
        {
            return Ok(new { Message = "Prompt is not ready yet or is invalid.", IsSuccess = false });
        }
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


    [HttpPost]
    public async Task<IActionResult> CreateImageByFluxOptimized([FromBody] CreateImageByPrompt request)
    {
        var promptId =
            await _simpleTextPromptService.CreateFluxOptimizedAsync(request.Prompt);
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
    public async Task<IActionResult> CreateFluxPromptWithSeedAndCustomPrompt(
        [FromBody] CreateBySeedAndPromptRequest request)
    {
        var result =
            await _simpleTextPromptService.CreateFluxPromptWithSeedAndCustomPrompt(request.Prompt, request.Seed);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetPromptImageUrlByPromptId([FromQuery] string promptId)
    {
        var imageUrl = await _simpleTextPromptService.GetImageByPromptId(promptId);
        var publicUrl = await _cloudinaryService.UploadSingleFileAsync(imageUrl);
        return Ok(new { PublicUrl = publicUrl });
    }

    [HttpGet]
    public async Task<IActionResult> GetFluxStyleChangeImageByPromptId([FromQuery] string promptId)
    {
        var status = await _simpleTextPromptService.GetPromptStatus(promptId);
        if (status)
        {
            var imageUrl = await _simpleTextPromptService.GetFluxStyleChangeImageByPromptId(promptId);
            var publicUrl = await _cloudinaryService.UploadSingleFileAsync(imageUrl);
            return Ok(new { PublicUrl = publicUrl, IsSuccess = true });
        }
        else
        {
            return Ok(new { Message = "Prompt is not ready yet or is invalid.", IsSuccess = false });
        }
    }

    [HttpPost]
    public async Task<IActionResult> UploadImageByFile([FromForm] IFormFile file)
    {
        // 檢查檔案是否為 null 或空
        if (file == null || file.Length == 0)
            return BadRequest("未收到檔案。");

        // 檢查圖片副檔名
        var fileType = Path.GetExtension(file.FileName).ToLower();
        var supportedTypes = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        if (!supportedTypes.Contains(fileType))
            return BadRequest("Unsupported file type.");

        var uploadResult = await _simpleTextPromptService.UploadImageToComfyUI(file);
        return Ok(uploadResult);
    }

    [HttpGet]
    public async Task<IActionResult> GetInputUploadImageUrl([FromQuery] string imageName, string subfolder = "")
    {
        var imageUrl = _simpleTextPromptService.GetUploadImageUrl(imageName, subfolder);
        var publicUrl = await _cloudinaryService.UploadSingleFileAsync(imageUrl);
        return Ok(new { PublicUrl = publicUrl });
    }


    [HttpPost]
    public async Task<IActionResult> CreatePromptWithSeedAndCustomPrompt(
        [FromBody] CreateBySeedAndPromptRequest request)
    {
        CreateFluxPromptResult result =
            await _simpleTextPromptService.CreateFluxPromptWithSeedAndCustomPrompt(request.Prompt, request.Seed);
        bool promptStatus = false;
        promptStatus = await _simpleTextPromptService.CheckPromptStatusWithRetry(result.PromptId);
        if (!promptStatus)
        {
            return BadRequest("系統忙碌中，請稍後再試。");
        }

        var imageUrl = await _simpleTextPromptService.GetImageByPromptId(result.PromptId);
        var publicUrl = await _cloudinaryService.UploadSingleFileAsync(imageUrl);
        return Ok(new
            { PromptId = result.PromptId, Prompt = request.Prompt, Seed = request.Seed, publicUrl = publicUrl });
    }

    [HttpPost]
    public async Task<IActionResult> CreatePromptWithSeedAndCustomPromptAndSize(
        [FromBody] CreateBySeedAndPromptAndSizeRequest request)
    {
        var result =
            await _simpleTextPromptService.CreateFluxPromptWithSeedAndCustomPromptAndSize(request.Prompt, request.Seed,
                request.Width, request.Height);
        return Ok(new
        {
            PromptId = result, Prompt = request.Prompt, Seed = request.Seed, Width = request.Width,
            Height = request.Height
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateStyleChangeImageByPrompt(
        [FromBody] CreateStyleChangeImageByPromptRequest request)
    {
        var promptId = await _simpleTextPromptService.CreateFluxStyleChangeImageWithStyleAndSeed(request.ImageName,
            request.Style, request.Seed, request.Width, request.Height);
        return Ok(new
        {
            PromptId = promptId, ImageName = request.ImageName, Seed = request.Seed, Width = request.Width,
            Height = request.Height
        });
    }
}