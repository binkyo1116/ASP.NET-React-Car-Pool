using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carpool.Infrastructure.src.Infrastructure.Persistence.Migrations
{
    public partial class renameProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "Events",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Events",
                newName: "Nom");
        }
    }
}
