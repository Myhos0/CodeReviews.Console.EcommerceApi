using EcommerceAPI.Enums;

namespace EcommerceAPI.Exceptions;

public sealed record Error(string Code, string? Description = null, ErrorType Type = ErrorType.Failure)
{
    public static readonly Error None = new(string.Empty);
}