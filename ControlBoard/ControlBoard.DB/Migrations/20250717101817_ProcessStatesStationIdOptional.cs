using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControlBoard.DB.Migrations
{
    /// <inheritdoc />
    public partial class ProcessStatesStationIdOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_process_states_stations_station_id",
                table: "process_states");

            migrationBuilder.AlterColumn<int>(
                name: "station_id",
                table: "process_states",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_process_states_stations_station_id",
                table: "process_states",
                column: "station_id",
                principalTable: "stations",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_process_states_stations_station_id",
                table: "process_states");

            migrationBuilder.AlterColumn<int>(
                name: "station_id",
                table: "process_states",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_process_states_stations_station_id",
                table: "process_states",
                column: "station_id",
                principalTable: "stations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
