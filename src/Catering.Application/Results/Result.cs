namespace Catering.Application.Results;

public abstract class Result
{
    public bool IsSuccess { get; protected set; }
    public ErrorType? Type { get; protected set; }
    public ICollection<string> ErrorCodes { get; protected set; } = Array.Empty<string>();

    public static EmptyResult Success() => new() { IsSuccess = true };

    public static Result<T> Success<T>(T value) => new() { IsSuccess = true, Value = value };

    public static EmptyResult NotFound(params string[] errors) => Error(ErrorType.NotFound, errors);

    public static EmptyResult ValidationError(params string[] errors) => Error(ErrorType.ValidationError, errors);

    public static EmptyResult UnknownError(params string[] errors) => Error(ErrorType.Unknown, errors);

    public static EmptyResult Error(ErrorType type, params string[] errors)
        => new() { IsSuccess = false, Type = type, ErrorCodes = errors };

    public static Result<T> From<T>(Result result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return new Result<T>
        {
            ErrorCodes = result.ErrorCodes,
            Type = result.Type,
            IsSuccess = result.IsSuccess
        };
    }
}

public class Result<T> : Result
{
    public T Value { get; set; }

    public static implicit operator Result<T>(EmptyResult result) => From<T>(result);
    public Result<T> ToResult(EmptyResult result) => From<T>(result);
}

public class EmptyResult : Result { }
