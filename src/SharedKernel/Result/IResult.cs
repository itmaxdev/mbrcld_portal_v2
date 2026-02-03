namespace Mbrcld.SharedKernel.Result
{
    public interface IResult
    {
        bool IsFailure { get; }
        bool IsSuccess { get; }
    }
}
