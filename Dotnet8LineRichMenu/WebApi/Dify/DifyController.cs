using Dotnet8LineRichMenu.Models;
using Dotnet8LineRichMenu.Services.Dify;
using Dotnet8LineRichMenu.Services.Dify.Dtos;
using Dotnet8LineRichMenu.WebApi.Dify.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Dotnet8LineRichMenu.WebApi.Dify;

[Route("api/[controller]/[action]")]
[ApiController]
public class DifyController : ControllerBase
{
    private readonly DifyService _difyService;
    private readonly string _difyUserId;

    public DifyController(DifyService difyService, IConfiguration configuration)
    {
        _difyService = difyService;
        _difyUserId = configuration["DifyUserId"];
    }

    [HttpPost]
    public async Task<IActionResult> CreateDifyImageByPrompt([FromBody] CreateDifyImageByPromptRequest request)
    {
        var inputs = new Dictionary<string, object>();
        inputs.Add("prompt", request.Prompt);
        inputs.Add("width", request.Width);
        inputs.Add("height", request.Height);
        inputs.Add("seed", request.Seed);
        var runWorkflowRequest = new DifyWorkflowRequest()
        {
            Inputs = inputs,
            ResponseMode = "blocking",
            User = _difyUserId
        };
        var response = await _difyService.CreateDifyImageByPrompt(runWorkflowRequest);
        var apiResponse = new ApiResponse();
        apiResponse.Body = response.Data.Outputs;
        apiResponse.Message = "Image created successfully";
        return Ok(apiResponse);
    }
    
    
}