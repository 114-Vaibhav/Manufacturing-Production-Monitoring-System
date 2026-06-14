using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModelsWithSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocationName",
                table: "Locations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DowntimeReason",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DowntimeReason", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DowntimeReason",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Machine Breakdown" },
                    { 2, "Power Failure" },
                    { 3, "Material Shortage" },
                    { 4, "Tool Change" },
                    { 5, "Planned Maintenance" },
                    { 6, "Unplanned Maintenance" },
                    { 7, "Operator Absence" },
                    { 8, "Quality Issue" },
                    { 9, "Setup Changeover" },
                    { 10, "Network Failure" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "LocationId", "FloorNo", "LocationName", "Status", "TerminalNo" },
                values: new object[,]
                {
                    { 1, "1", "Assembly Line", 0, "A1" },
                    { 2, "1", "Packaging Line", 0, "A2" },
                    { 3, "1", "Quality Control", 0, "A3" }
                });

            migrationBuilder.InsertData(
                table: "Machines",
                columns: new[] { "MachineId", "LastMaintenanceDate", "LocationId", "MachineCode", "MachineName", "Status" },
                values: new object[,]
                {
                    { 1, null, 1, "ASM-001", "Conveyor Belt A", 0 },
                    { 2, null, 1, "ASM-002", "Robotic Arm A", 0 },
                    { 3, null, 2, "PKG-001", "Packaging Machine A", 0 },
                    { 4, null, 2, "PKG-002", "Labeling Machine A", 0 },
                    { 5, null, 3, "QC-001", "Vision Inspection System", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DowntimeReason");

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "MachineId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "MachineId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "MachineId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "MachineId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Machines",
                keyColumn: "MachineId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "LocationId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "LocationId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Locations",
                keyColumn: "LocationId",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "LocationName",
                table: "Locations");
        }
    }
}
