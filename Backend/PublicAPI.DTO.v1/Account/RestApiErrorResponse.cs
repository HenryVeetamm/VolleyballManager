using System.Net;

namespace PublicAPI.DTO.v1.Account;

public class RestApiErrorResponse
{
    
    public string Type { get; set; } = "https://datatracker.ietf.org/doc/html/rfc7231#section";
    public string Title { get; set; } = "App error";
    public HttpStatusCode Status { get; set; } = default!;
    public string TraceId { get; set; } = default!;
    public Dictionary<string, List<string>> Errors { get; set; } = new();
    public RestApiErrorResponse(HttpStatusCode status, string traceId, string errorKey, List<string> ErrorMessages)
    {
        Status = status;
        TraceId = traceId;
        Errors[errorKey] = ErrorMessages;
    }
    
    public RestApiErrorResponse(HttpStatusCode status, string traceId)
    {
        Status = status;
        TraceId = traceId;
    }

    public void AddError(string errorKey, List<string> errorMessages)
    {
        Errors[errorKey] = errorMessages;
    }

}