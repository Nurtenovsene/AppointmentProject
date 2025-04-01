using Appointment.Business.Core.Results;

namespace TourOperator.Business.Core.Results;

public interface IResult
{
    bool IsSuccess { get; }
}

public class Result : IResult
{
    public bool IsSuccess { get; }
    public List<Error>? Errors { get; }

    private Result(bool isSuccess, List<Error>? errors)
    {
        if ((isSuccess && errors != null && errors.Count > 0) || (!isSuccess && (errors == null || errors.Count == 0)))
        {
            throw new InvalidOperationException("Invalid state: Success should not have errors, and failure must have at least one error.");
        }

        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success() => new(true, null);




    public static implicit operator Result(Error error) => new(false, [error]);

    public static implicit operator Result(List<Error> errors) => new(false, errors);
}

public class Result<T> : IResult
{
    public bool IsSuccess { get; }
    public List<Error>? Errors { get; }
    public T? Body { get; }
    public int? Count { get; }

    private Result(bool isSuccess, T? body, List<Error>? errors, int? count = null)
    {
        if ((isSuccess && errors != null && errors.Count > 0) || (!isSuccess && (errors == null || errors.Count == 0)))
        {
            throw new InvalidOperationException("Invalid state: Success should not have errors, and failure must have at least one error.");
        }
        IsSuccess = isSuccess;
        Body = body;
        Errors = errors;
        Count = count;
    }

    public static Result<T> Success(T body, int count) => new(true, body, null, count);

    public static Result<T> Success(T body) => new(true, body, null, null);

    public static implicit operator Result<T>(Error error) => new(false, default, [error], null);

    public static implicit operator Result<T>(List<Error> errors) => new(false, default, errors, null);
}