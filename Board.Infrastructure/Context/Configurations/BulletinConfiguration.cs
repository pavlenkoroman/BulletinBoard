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
            .Property(bulletin => bulletin.IsActive)
            .HasDefaultValue(true);

        builder
            .HasIndex(bulletin => bulletin.Number);

        builder
            .HasIndex(bulletin => bulletin.UserId);

        builder
            .HasIndex(bulletin => bulletin.CreatedDate);

        builder
            .HasIndex(b => b.ExpirationDate);

        builder
            .HasIndex(b => b.Text);

        builder
            .HasIndex(b => b.Rating);

        builder
            .ToTable("bulletins");
    }
}
