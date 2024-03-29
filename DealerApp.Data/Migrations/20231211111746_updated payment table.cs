﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DealerApp.Model.Models
{
    /// <inheritdoc />
    public partial class updatedpaymenttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentProofImg",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentProofImg",
                table: "Payment");
        }
    }
}
