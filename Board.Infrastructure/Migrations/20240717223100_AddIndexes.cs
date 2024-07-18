using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Board.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_users_is_admin",
                table: "users",
                column: "is_admin");

            migrationBuilder.CreateIndex(
                name: "ix_users_name",
                table: "users",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_bulletins_created_date",
                table: "bulletins",
                column: "created_date");

            migrationBuilder.CreateIndex(
                name: "ix_bulletins_expiration_date",
                table: "bulletins",
                column: "expiration_date");

            migrationBuilder.CreateIndex(
                name: "ix_bulletins_number",
                table: "bulletins",
                column: "number");

            migrationBuilder.CreateIndex(
                name: "ix_bulletins_rating",
                table: "bulletins",
                column: "rating");

            migrationBuilder.CreateIndex(
                name: "ix_bulletins_text",
                table: "bulletins",
                column: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_users_is_admin",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_users_name",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_bulletins_created_date",
                table: "bulletins");

            migrationBuilder.DropIndex(
                name: "ix_bulletins_expiration_date",
                table: "bulletins");

            migrationBuilder.DropIndex(
                name: "ix_bulletins_number",
                table: "bulletins");

            migrationBuilder.DropIndex(
                name: "ix_bulletins_rating",
                table: "bulletins");

            migrationBuilder.DropIndex(
                name: "ix_bulletins_text",
                table: "bulletins");
        }
    }
}
