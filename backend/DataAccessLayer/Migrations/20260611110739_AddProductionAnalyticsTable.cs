using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddProductionAnalyticsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionAnalyticss_Machines_MachineId",
                table: "ProductionAnalyticss");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductionAnalyticss",
                table: "ProductionAnalyticss");

            migrationBuilder.RenameTable(
                name: "ProductionAnalyticss",
                newName: "ProductionAnalytics");

            migrationBuilder.RenameIndex(
                name: "IX_ProductionAnalyticss_MachineId",
                table: "ProductionAnalytics",
                newName: "IX_ProductionAnalytics_MachineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductionAnalytics",
                table: "ProductionAnalytics",
                column: "AnalyticsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionAnalytics_Machines_MachineId",
                table: "ProductionAnalytics",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "MachineId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionAnalytics_Machines_MachineId",
                table: "ProductionAnalytics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductionAnalytics",
                table: "ProductionAnalytics");

            migrationBuilder.RenameTable(
                name: "ProductionAnalytics",
                newName: "ProductionAnalyticss");

            migrationBuilder.RenameIndex(
                name: "IX_ProductionAnalytics_MachineId",
                table: "ProductionAnalyticss",
                newName: "IX_ProductionAnalyticss_MachineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductionAnalyticss",
                table: "ProductionAnalyticss",
                column: "AnalyticsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionAnalyticss_Machines_MachineId",
                table: "ProductionAnalyticss",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "MachineId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
