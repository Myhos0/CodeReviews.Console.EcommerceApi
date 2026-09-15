using EcommerceAPI.Enums;

namespace EcommerceAPI.Exceptions;

public static class EcommerceErrors
{
    public static Error BadRequest(string code,string message) => 
        new($"{code}.BadRequest", message, ErrorType.Validation);

    public static Error NotFound(string code, string message) =>
        new($"{code}.NotFound", message, ErrorType.NotFound);

    public static Error Conflict(string code, string message) =>
        new($"{code}.Conflict", message, ErrorType.Conflict);

    public static Error BusinessRule(string code, string message) =>
        new ($"{code}.BussinesRule",message, ErrorType.BusinessRule);
}