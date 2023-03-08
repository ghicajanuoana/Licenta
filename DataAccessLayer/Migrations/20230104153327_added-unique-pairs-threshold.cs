using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class addeduniquepairsthreshold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Thresholds_DeviceReadingTypes_DeviceReadingTypeId",
                table: "Thresholds");

            migrationBuilder.DropForeignKey(
                name: "FK_Thresholds_DeviceTypes_DeviceTypeId",
                table: "Thresholds");

            migrationBuilder.DropIndex(
                name: "IX_Thresholds_DeviceTypeId",
                table: "Thresholds");

            migrationBuilder.CreateIndex(
                name: "IX_Thresholds_DeviceTypeId_DeviceReadingTypeId",
                table: "Thresholds",
                columns: new[] { "DeviceTypeId", "DeviceReadingTypeId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Thresholds_DeviceReadingTypes_DeviceReadingTypeId",
                table: "Thresholds",
                column: "DeviceReadingTypeId",
                principalTable: "DeviceReadingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Thresholds_DeviceTypes_DeviceTypeId",
                table: "Thresholds",
                column: "DeviceTypeId",
                principalTable: "DeviceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Thresholds_DeviceReadingTypes_DeviceReadingTypeId",
                table: "Thresholds");

            migrationBuilder.DropForeignKey(
                name: "FK_Thresholds_DeviceTypes_DeviceTypeId",
                table: "Thresholds");

            migrationBuilder.DropIndex(
                name: "IX_Thresholds_DeviceTypeId_DeviceReadingTypeId",
                table: "Thresholds");

            migrationBuilder.CreateIndex(
                name: "IX_Thresholds_DeviceTypeId",
                table: "Thresholds",
                column: "DeviceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Thresholds_DeviceReadingTypes_DeviceReadingTypeId",
                table: "Thresholds",
                column: "DeviceReadingTypeId",
                principalTable: "DeviceReadingTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Thresholds_DeviceTypes_DeviceTypeId",
                table: "Thresholds",
                column: "DeviceTypeId",
                principalTable: "DeviceTypes",
                principalColumn: "Id");
        }
    }
}
