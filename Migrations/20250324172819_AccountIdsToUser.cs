using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projektBanka.Migrations
{
    /// <inheritdoc />
    public partial class AccountIdsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Account_DestinationAccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Account_PayerAccountId",
                table: "Transactions");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Account_DestinationAccountId",
                table: "Transactions",
                column: "DestinationAccountId",
                principalTable: "Account",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Account_PayerAccountId",
                table: "Transactions",
                column: "PayerAccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Account_DestinationAccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Account_PayerAccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CommonAccountId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreditAccountId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SavingsAccountId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Account_DestinationAccountId",
                table: "Transactions",
                column: "DestinationAccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Account_PayerAccountId",
                table: "Transactions",
                column: "PayerAccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
