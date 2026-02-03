namespace Mbrcld.SharedKernel.Result
{
    public struct Result : IResult
    {
        public bool IsFailure { get; }
        public bool IsSuccess => !IsFailure;
        public string[] Errors { get; }

        private Result(bool isFailure, string[] errors)
        {
            this.IsFailure = isFailure;
            this.Errors = errors ?? new string[0];
        }

        public static Result Failure(params string[] errors)
        {
            return new Result(true, errors);
        }

        public static Result<T> Failure<T>(params string[] errors)
        {
            return new Result<T>(true, errors, default);
        }

        public static Result Success()
        {
            return new Result(false, null);
        }

        public static Result<T> Success<T>(T value)
        {
            return new Result<T>(false, default, value);
        }
    }
}
