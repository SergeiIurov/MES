using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlBoard.DB.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDataTypeForAddControlBoardData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "data",
                table: "control_board_data",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "data",
                table: "control_board_data",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
