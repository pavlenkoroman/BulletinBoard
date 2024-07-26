using Board.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Board.Infrastructure.Context.Configurations;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder
            .HasKey(rating => new { rating.UserId, rating.BulletinId });

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(rating => rating.UserId);

        builder
            .HasOne<Bulletin>()
            .WithMany()
            .HasForeignKey(rating => rating.BulletinId);

        builder
            .ToTable("ratings");
    }
}
