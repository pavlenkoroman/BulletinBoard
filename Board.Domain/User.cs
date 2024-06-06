namespace Board.Domain;

public sealed class User
{
    public Guid Id { get; private init; }
    public string Name { get; private set; }
    public bool IsAdmin { get; private init; }

    public User(Guid id, string name, bool isAdmin)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(id, Guid.Empty);
        ValidateName(name);
        Id = id;
        Name = name;
        IsAdmin = isAdmin;
    }

    public static User Create(string name, bool isAdmin)
    {
        return new User(Guid.NewGuid(), name, isAdmin);
    }

    public void UpdateName(string name)
    {
        ValidateName(name);
        Name = name;
    }

    private void ValidateName(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        ArgumentOutOfRangeException.ThrowIfLessThan(name.Length, 5);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(name.Length, 100);
    }
}
