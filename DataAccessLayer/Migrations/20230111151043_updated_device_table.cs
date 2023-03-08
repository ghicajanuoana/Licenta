using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class updateddevicetable : Migration
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

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPingedTs",
                table: "Devices",
                type: "datetime2",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Thresholds_DeviceReadingTypes_DeviceReadingTypeId",
                table: "Thresholds");

            migrationBuilder.DropForeignKey(
                name: "FK_Thresholds_DeviceTypes_DeviceTypeId",
                table: "Thresholds");

            migrationBuilder.DropColumn(
                name: "LastPingedTs",
                table: "Devices");

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
    }
}
