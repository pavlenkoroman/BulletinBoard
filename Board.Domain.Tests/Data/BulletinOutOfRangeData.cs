using Xunit;

namespace Board.Domain.Tests.Data;

internal class BulletinOutOfRangeData : TheoryData<Guid, int, Guid, string, Photo, int, DateTime, DateTime>
{
    public BulletinOutOfRangeData()
    {
        var photo = new Photo(
            Guid.NewGuid(),
            "/a",
            new Uri("https://example.com"),
            "/a_r",
            new Uri("https://resized.com"));

        Add(Guid.Empty,
            0,
            Guid.NewGuid(),
            new string('a', 50),
            photo,
            0,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1));
        Add(Guid.NewGuid(),
            0,
            Guid.Empty,
            new string('a', 50),
            photo,
            0,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1));
        Add(Guid.NewGuid(),
            0,
            Guid.NewGuid(),
            new string('a', 49),
            photo,
            0,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1));
        Add(Guid.NewGuid(),
            0,
            Guid.Empty,
            new string('a', 1001),
            photo,
            0,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1));
        Add(Guid.NewGuid(),
            0,
            Guid.NewGuid(),
            new string('a', 50),
            photo,
            0,
            new DateTime(),
            DateTime.UtcNow);
        Add(Guid.NewGuid(),
            0,
            Guid.NewGuid(),
            new string('a', 51),
            photo,
            0,
            DateTime.UtcNow,
            new DateTime());
        Add(Guid.NewGuid(),
            0,
            Guid.NewGuid(),
            new string('a', 50),
            photo,
            0,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(-1));
    }
}
