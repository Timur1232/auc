using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Migrations
{
    /// <inheritdoc />
    public partial class entity_framework_is_piece_of_shit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "tag_id",
                table: "lots");

            migrationBuilder.DropColumn(
                name: "user_login",
                table: "lots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "tag_id",
                table: "lots",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<string>(
                name: "user_login",
                table: "lots",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
