﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealerApp.Model.Models
{
    /// <inheritdoc />
    public partial class Statww : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Userstbl_Statustbl_StatusId",
                table: "Userstbl");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Userstbl",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Userstbl_Statustbl_StatusId",
                table: "Userstbl",
                column: "StatusId",
                principalTable: "Statustbl",
                principalColumn: "StatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Userstbl_Statustbl_StatusId",
                table: "Userstbl");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Userstbl",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Userstbl_Statustbl_StatusId",
                table: "Userstbl",
                column: "StatusId",
                principalTable: "Statustbl",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
