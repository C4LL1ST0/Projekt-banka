using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projektBanka.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAccountIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommonAccountId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreditAccountId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SavingsAccountId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CommonAccountId",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreditAccountId",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SavingsAccountId",
                table: "Users",
                type: "TEXT",
                nullable: true);
        }
    }
}
