using Core.Models.Interfaces;

namespace Core.Models
{
    public abstract record Result<TSuccess, TError> : IResult
    {
        public sealed record Success(TSuccess Value) : Result<TSuccess, TError>;
        public sealed record Failure(TError Error) : Result<TSuccess, TError>;

        public static Result<TSuccess, TError> Ok(TSuccess value) => new Success(value);
        public static Result<TSuccess, TError> Fail(TError error) => new Failure(error);

        public bool IsSuccess => this is Success;
        public bool IsFailure => this is Failure;

        public bool TryGetSuccess(out TSuccess value)
        {
            if (this is Success s)
            {
                value = s.Value;
                return true;
            }
            value = default!;
            return false;
        }

        public bool TryGetError(out TError error)
        {
            if (this is Failure f)
            {
                error = f.Error;
                return true;
            }
            error = default!;
            return false;
        }
    }
}