using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Board.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRatingToUserAndBulletinRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_ratings_bulletin_id",
                table: "ratings");

            migrationBuilder.DropIndex(
                name: "ix_ratings_user_id",
                table: "ratings");

            migrationBuilder.CreateIndex(
                name: "ix_ratings_bulletin_id",
                table: "ratings",
                column: "bulletin_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_ratings_bulletin_id",
                table: "ratings");

            migrationBuilder.CreateIndex(
                name: "ix_ratings_bulletin_id",
                table: "ratings",
                column: "bulletin_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_ratings_user_id",
                table: "ratings",
                column: "user_id",
                unique: true);
        }
    }
}
