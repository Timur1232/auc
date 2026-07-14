using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Migrations
{
    /// <inheritdoc />
    public partial class lots_tags_to_tag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LotTag");

            migrationBuilder.AddColumn<uint>(
                name: "tag_id",
                table: "lots",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "tagid",
                table: "lots",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.CreateIndex(
                name: "IX_lots_tagid",
                table: "lots",
                column: "tagid");

            migrationBuilder.AddForeignKey(
                name: "FK_lots_tags_tagid",
                table: "lots",
                column: "tagid",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_lots_tags_tagid",
                table: "lots");

            migrationBuilder.DropIndex(
                name: "IX_lots_tagid",
                table: "lots");

            migrationBuilder.DropColumn(
                name: "tag_id",
                table: "lots");

            migrationBuilder.DropColumn(
                name: "tagid",
                table: "lots");

            migrationBuilder.CreateTable(
                name: "LotTag",
                columns: table => new
                {
                    lotsid = table.Column<uint>(type: "INTEGER", nullable: false),
                    tagsid = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LotTag", x => new { x.lotsid, x.tagsid });
                    table.ForeignKey(
                        name: "FK_LotTag_lots_lotsid",
                        column: x => x.lotsid,
                        principalTable: "lots",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LotTag_tags_tagsid",
                        column: x => x.tagsid,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LotTag_tagsid",
                table: "LotTag",
                column: "tagsid");
        }
    }
}
