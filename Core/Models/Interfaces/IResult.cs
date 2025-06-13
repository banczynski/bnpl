namespace Core.Models.Interfaces
{
    public interface IResult
    {
        bool IsSuccess { get; }
        bool IsFailure { get; }
    }
}
