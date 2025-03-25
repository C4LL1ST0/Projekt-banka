using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projektBanka.Migrations
{
    /// <inheritdoc />
    public partial class Transactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavingsAccounts_Users_UserId",
                table: "SavingsAccounts");

            migrationBuilder.DropTable(
                name: "CommonAccounts");

            migrationBuilder.DropTable(
                name: "CreditAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SavingsAccounts",
                table: "SavingsAccounts");

            migrationBuilder.RenameTable(
                name: "SavingsAccounts",
                newName: "Account");

            migrationBuilder.RenameIndex(
                name: "IX_SavingsAccounts_UserId",
                table: "Account",
                newName: "IX_Account_UserId");

            migrationBuilder.AddColumn<double>(
                name: "Ceiling",
                table: "Account",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Account",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Account",
                type: "TEXT",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Interest",
                table: "Account",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "InterestFreePeriod",
                table: "Account",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Money",
                table: "Account",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SavingsAccount_Interest",
                table: "Account",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                table: "Account",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PayerAccountId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DestinationAccountId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Amount = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Account_DestinationAccountId",
                        column: x => x.DestinationAccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_Account_PayerAccountId",
                        column: x => x.PayerAccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DestinationAccountId",
                table: "Transaction",
                column: "DestinationAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_PayerAccountId",
                table: "Transaction",
                column: "PayerAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Users_UserId",
                table: "Account",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Users_UserId",
                table: "Account");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Account",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Ceiling",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Interest",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "InterestFreePeriod",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Money",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "SavingsAccount_Interest",
                table: "Account");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "SavingsAccounts");

            migrationBuilder.RenameIndex(
                name: "IX_Account_UserId",
                table: "SavingsAccounts",
                newName: "IX_SavingsAccounts_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SavingsAccounts",
                table: "SavingsAccounts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CommonAccounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommonAccounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditAccounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditAccounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommonAccounts_UserId",
                table: "CommonAccounts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreditAccounts_UserId",
                table: "CreditAccounts",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SavingsAccounts_Users_UserId",
                table: "SavingsAccounts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
