using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carpool.Infrastructure.src.Persistence.Migrations
{
    public partial class PhotoEntityAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoId",
                table: "Events",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_PhotoId",
                table: "Events",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Photo_PhotoId",
                table: "Events",
                column: "PhotoId",
                principalTable: "Photo",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Photo_PhotoId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "Photo");

            migrationBuilder.DropIndex(
                name: "IX_Events_PhotoId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Events");
        }
    }
}
