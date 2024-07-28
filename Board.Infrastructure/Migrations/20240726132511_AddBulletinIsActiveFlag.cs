using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Board.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBulletinIsActiveFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "bulletins",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                table: "bulletins");
        }
    }
}
