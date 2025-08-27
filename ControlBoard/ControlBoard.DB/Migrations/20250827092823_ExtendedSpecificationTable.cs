using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlBoard.DB.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedSpecificationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "chassis_assembly_start_data",
                table: "specification",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "date_installation_cabin_on_chassis",
                table: "specification",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "chassis_assembly_start_data",
                table: "specification");

            migrationBuilder.DropColumn(
                name: "date_installation_cabin_on_chassis",
                table: "specification");
        }
    }
}
