namespace EcommerceAPI.Exceptions;

public class Result
{
    private Result(bool isSucces,Error error) 
    {
        if(isSucces && error != Error.None || 
            !isSucces && error == Error.None) 
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSucces = isSucces;
        Error = error;
    }

    public bool IsSucces { get; }

    public bool IsFailure => !IsSucces;

    public Error Error { get; }

    public static Result Succes() => new(true, Error.None);

    public static Result Failure(Error error) => new(false,error);

}