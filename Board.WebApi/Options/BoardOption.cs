using Board.Application.Options;

namespace Board.WebApi.Options;

public class BoardOption : IBoardOption
{
    public const string Name = "BoardOptions";
    public int MaxBulletinsPerUser { get; init; }
}
