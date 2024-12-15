namespace Lib.Results;

public class Result
    {
        public bool IsFaulted { get; init; }

        public bool IsSuccess => !IsFaulted;

        public string Message { get; init; }

        public ErrorCode ErrorCode { get; init; }

        public Result()
        {
        }

        public Result(Result originalResult)
        {
            ArgumentNullException.ThrowIfNull(originalResult);

            IsFaulted = originalResult.IsFaulted;
            Message = originalResult.Message;
            ErrorCode = originalResult.ErrorCode;
        }

        public static Result Success()
        {
            return new Result();
        }

        public static Result Error(string message, ErrorCode errorCode = ErrorCode.GeneralError)
        {
            var result = new Result
            {
                IsFaulted = true,
                Message = message,
                ErrorCode = errorCode
            };

            return result;
        }
    }

public class Result<T> : Result
{
    public T Data { get; init; }

    public Result()
    {
    }

    public Result(T data)
    {
        Data = data;
    }

    public Result(Result originalResult)
        : base(originalResult)
    {
    }
}
