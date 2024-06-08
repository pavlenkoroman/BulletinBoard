using Xunit;

namespace Board.Domain.Tests.Data;

internal class BulletinOutOfRangeData : TheoryData<Guid, int, Guid, string, Uri, int, DateTime, DateTime>
{
    public BulletinOutOfRangeData()
    {
        Add(Guid.Empty, 
            0, 
            Guid.NewGuid(), 
            new string('a', 50), 
            new Uri("https://example.com/a.jpg"), 
            0, 
            DateTime.UtcNow, 
            DateTime.UtcNow.AddDays(1));
        Add(Guid.NewGuid(),
            0,
            Guid.Empty,
            new string('a', 50),
            new Uri("https://example.com/a.jpg"),
            0,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1));
        Add(Guid.NewGuid(), 
            0, 
            Guid.NewGuid(), 
            new string('a', 49),
            new Uri("https://example.com/a.jpg"), 
            0, 
            DateTime.UtcNow, 
            DateTime.UtcNow.AddDays(1));
        Add(Guid.NewGuid(), 
            0, 
            Guid.Empty, 
            new string('a', 1001),
            new Uri("https://example.com/a.jpg"), 
            0, 
            DateTime.UtcNow, 
            DateTime.UtcNow.AddDays(1));
        Add(Guid.NewGuid(), 
            0, 
            Guid.NewGuid(), 
            new string('a', 50),
            new Uri("https://example.com/a.jpg"), 
            0, 
            new DateTime(), 
            DateTime.UtcNow);
        Add(Guid.NewGuid(), 
            0, 
            Guid.NewGuid(), 
            new string('a', 51),
            new Uri("https://example.com/a.jpg"), 
            0, 
            DateTime.UtcNow, 
            new DateTime());
        Add(Guid.NewGuid(), 
            0, 
            Guid.NewGuid(), 
            new string('a', 50),
            new Uri("https://example.com/a.jpg"), 
            0, 
            DateTime.UtcNow, 
            DateTime.UtcNow.AddDays(-1));
    }
}

