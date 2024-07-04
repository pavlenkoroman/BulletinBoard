using Board.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Context.Configurations;

public class BulletinConfiguration : IEntityTypeConfiguration<Bulletin>
{
    public void Configure(EntityTypeBuilder<Bulletin> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder
            .HasKey(bulletin => bulletin.Id);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(bulletin => bulletin.UserId);

        builder
            .Property(bulletin => bulletin.Number)
            .ValueGeneratedOnAdd();

        builder
            .Property(bulletin => bulletin.Photo)
            .HasColumnType("jsonb");

        builder
            .ToTable("bulletins");
    }
}
