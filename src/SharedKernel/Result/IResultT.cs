namespace Mbrcld.SharedKernel.Result
{
    public interface IResult<out T> : IResult, IValue<T>
    {
    }

    public interface IValue<out T>
    {
        T Value { get; }
    }
}
