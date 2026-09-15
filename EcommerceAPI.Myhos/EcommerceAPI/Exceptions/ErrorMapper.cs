using EcommerceAPI.Enums;

namespace EcommerceAPI.Exceptions;

public static class ErrorMapper
{
    public static (int StatusCode, string Title) Map(this Error error) => error.Type switch
    {
        ErrorType.NotFound => (StatusCodes.Status404NotFound, "Resource not found"),
        ErrorType.Validation => (StatusCodes.Status400BadRequest, "Invalid request"),
        ErrorType.Conflict => (StatusCodes.Status409Conflict, "Conflict"),
        ErrorType.BusinessRule => (StatusCodes.Status400BadRequest, "Business rule violated"),
        _ => (StatusCodes.Status500InternalServerError, "An unexpected errro occurred")
    };
}