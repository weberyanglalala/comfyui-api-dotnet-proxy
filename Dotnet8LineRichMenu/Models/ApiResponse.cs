namespace Dotnet8LineRichMenu.Models;

public class ApiResponse
{
    public ApiResponse()
    {
        
    }
    public ApiResponse(object body, string message = "")
    {
        Body = body;
        Message = !string.IsNullOrWhiteSpace(message) ? message : "Operation Done Successfully.";
    }

    public object Body { get; set; }
    public string Message { get; set; }
}