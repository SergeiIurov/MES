using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlBoard.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsDisabledFieldInAreaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_disabled",
                table: "areas",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_disabled",
                table: "areas");
        }
    }
}
