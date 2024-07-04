using Board.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Context.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder
            .HasKey(user => user.Id);

        builder
            .Property(user => user.Name)
            .IsRequired();

        builder
            .Property(user => user.IsAdmin)
            .IsRequired();

        builder
            .ToTable("users");
    }
}
