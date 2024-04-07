﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class deletedDeviceReadingTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeviceReadings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeviceReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    DeviceReadingTypeId = table.Column<int>(type: "int", nullable: false),
                    AlertType = table.Column<int>(type: "int", nullable: true),
                    IsAlertRead = table.Column<bool>(type: "bit", nullable: false),
                    ReceivedTs = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValueRead = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceReadings_DeviceReadingTypes_DeviceReadingTypeId",
                        column: x => x.DeviceReadingTypeId,
                        principalTable: "DeviceReadingTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DeviceReadings_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceReadings_DeviceId",
                table: "DeviceReadings",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceReadings_DeviceReadingTypeId",
                table: "DeviceReadings",
                column: "DeviceReadingTypeId");
        }
    }
}
