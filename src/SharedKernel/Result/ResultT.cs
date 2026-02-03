namespace Mbrcld.SharedKernel.Result
{
    public struct Result<T> : IResult<T>
    {
        public bool IsFailure { get; }
        public bool IsSuccess => !IsFailure;
        public string[] Errors { get; }
        public T Value { get; }

        internal Result(bool isFailure, string[] errors, T value)
        {
            this.IsFailure = isFailure;
            this.Errors = errors ?? new string[0];
            this.Value = value;
        }

        public static implicit operator Result<T>(T value)
        {
            if (value is Result<T> result)
            {
                return result;
            }

            return Result.Success(value);
        }

        public static implicit operator Result(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Result.Success();
            }
            else
            {
                return Result.Failure(result.Errors);
            }
        }
    }
}
