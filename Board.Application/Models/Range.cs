namespace Board.Application.Models;

public sealed record Range<T> where T : struct, IComparable<T>{
    public T? Start { get; }
    public T? End { get; }

    public Range(T? start, T? end)
    {
        if (start.HasValue && end.HasValue && start.Value.CompareTo(end.Value) >= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(end), 
                "Range end cannot be less or equal than range start");
        }

        Start = start;
        End = end;
    }
}
