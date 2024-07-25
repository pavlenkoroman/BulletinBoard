namespace Board.Application.Bulletins.Models;

public sealed record IntegerRange
{
    public int? Start { get; private init; }
    public int? End { get; private init; }

    public IntegerRange(int? start, int? end)
    {
        if (start is not null && end is not null && end < start)
        {
            throw new ArgumentOutOfRangeException(nameof(end), $"Range end cannot be less than range start");
        }

        Start = start;
        End = end;
    }
}
