using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NationalParkAPI.Migrations
{
    public partial class addElevationToTrail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Elevation",
                table: "trails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Elevation",
                table: "trails");
        }
    }
}
