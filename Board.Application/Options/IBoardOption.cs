namespace Board.Application.Options;

public interface IBoardOption
{
    public int MaxBulletinsPerUser { get; }
    public int BulletinsExpirationDays { get; }
}
