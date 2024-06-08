using Xunit;

namespace Board.Domain.Tests.Data;

public class BulletinValidData : TheoryData<int, Guid, string, Uri, DateTime>
{
    public BulletinValidData()
    {
        Add(0, Guid.NewGuid(), new string('a', 50), new Uri("https://example.com"), DateTime.UtcNow.AddDays(1));
    }
}
