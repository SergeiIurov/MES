using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ControlBoard.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddedScanningPointsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "scanning_points",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false, defaultValue: ""),
                    order_num = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    line_number = table.Column<int>(type: "integer", nullable: false),
                    code_ts = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scanning_points", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "scanning_points");
        }
    }
}
