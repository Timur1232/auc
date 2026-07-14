using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Migrations
{
    /// <inheritdoc />
    public partial class entity_framework_is_fr_hot_pile_of_garbage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lots_tags_tagid",
                table: "lots");

            migrationBuilder.DropForeignKey(
                name: "FK_lots_users_userlogin",
                table: "lots");

            migrationBuilder.DropIndex(
                name: "IX_lots_tagid",
                table: "lots");

            migrationBuilder.DropIndex(
                name: "IX_lots_userlogin",
                table: "lots");

            migrationBuilder.RenameColumn(
                name: "userlogin",
                table: "lots",
                newName: "user_login");

            migrationBuilder.RenameColumn(
                name: "tagid",
                table: "lots",
                newName: "tag_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "user_login",
                table: "lots",
                newName: "userlogin");

            migrationBuilder.RenameColumn(
                name: "tag_id",
                table: "lots",
                newName: "tagid");

            migrationBuilder.CreateIndex(
                name: "IX_lots_tagid",
                table: "lots",
                column: "tagid");

            migrationBuilder.CreateIndex(
                name: "IX_lots_userlogin",
                table: "lots",
                column: "userlogin");

            migrationBuilder.AddForeignKey(
                name: "FK_lots_tags_tagid",
                table: "lots",
                column: "tagid",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lots_users_userlogin",
                table: "lots",
                column: "userlogin",
                principalTable: "users",
                principalColumn: "login",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
