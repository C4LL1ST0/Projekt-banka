using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projektBanka.Migrations
{
    /// <inheritdoc />
    public partial class Student : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterestFreePeriod",
                table: "CreditAccounts");

            migrationBuilder.AddColumn<bool>(
                name: "IsStudent",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsStudent",
                table: "SavingsAccounts",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "ComputedMinus",
                table: "CreditAccounts",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStudent",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsStudent",
                table: "SavingsAccounts");

            migrationBuilder.DropColumn(
                name: "ComputedMinus",
                table: "CreditAccounts");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "InterestFreePeriod",
                table: "CreditAccounts",
                type: "TEXT",
                nullable: true);
        }
    }
}
