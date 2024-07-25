namespace Board.Application.Bulletins.Models;

public sealed record DateTimeRange
{
    public DateTime? Start { get; }
    public DateTime? End { get; }

    public DateTimeRange(DateTime? start, DateTime? end)
    {
        if (start is not null && end is not null && end < start)
        {
            throw new ArgumentOutOfRangeException(nameof(end), $"Range end cannot be less than range start");
        }

        Start = start;
        End = end;
    }
}
