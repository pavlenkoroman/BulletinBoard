using Xunit;

namespace Board.Domain.Tests.Data;

public class BulletinValidData : TheoryData<int, Guid, string, Photo, DateTime>
{
    public BulletinValidData()
    {
        var photo = new Photo(
            Guid.NewGuid(), 
            "/a", 
            new Uri("https://example.com"), 
            "/a_r",
            new Uri("https://resized.com"));
        Add(0, Guid.NewGuid(), new string('a', 50), photo, DateTime.UtcNow.AddDays(1));
    }
}
