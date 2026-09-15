namespace EcommerceAPI.Exceptions;

public class EcommerceException : Exception
{
    public Error Error { get; }

    public EcommerceException(Error error) : base(error.Description ?? error.Code)
    {
        Error = error;  
    }
}