using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projektBanka.Migrations
{
    /// <inheritdoc />
    public partial class RevertToOldDBSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_DestinationAccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_PayerAccountId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "CommonAccounts");

            migrationBuilder.DropTable(
                name: "CreditAccounts");

            migrationBuilder.DropTable(
                name: "SavingsAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Account");

            migrationBuilder.AddColumn<double>(
                name: "Ceiling",
                table: "Account",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ComputedBonus",
                table: "Account",
                type: "REAL",
                nullable: true);

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
                name: "SavingsAccount_Interest",
                table: "Account",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Account",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                table: "Account",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserId",
                table: "Account",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Users_UserId",
                table: "Account",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Account_Users_UserId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Account_DestinationAccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Account_PayerAccountId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Account",
                table: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Account_UserId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Ceiling",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "ComputedBonus",
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
                name: "SavingsAccount_Interest",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Account");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "Accounts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CommonAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommonAccounts_Accounts_Id",
                        column: x => x.Id,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommonAccounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Ceiling = table.Column<double>(type: "REAL", nullable: false),
                    Interest = table.Column<double>(type: "REAL", nullable: false),
                    InterestFreePeriod = table.Column<TimeSpan>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditAccounts_Accounts_Id",
                        column: x => x.Id,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditAccounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavingsAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ComputedBonus = table.Column<double>(type: "REAL", nullable: false),
                    Interest = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavingsAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavingsAccounts_Accounts_Id",
                        column: x => x.Id,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavingsAccounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
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

            migrationBuilder.CreateIndex(
                name: "IX_SavingsAccounts_UserId",
                table: "SavingsAccounts",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_DestinationAccountId",
                table: "Transactions",
                column: "DestinationAccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_PayerAccountId",
                table: "Transactions",
                column: "PayerAccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
