using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unity_Bank.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionApproval : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FailedTransactionAttempts",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsTransactionLocked",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TransactionPassword",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailedTransactionAttempts",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsTransactionLocked",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TransactionPassword",
                table: "AspNetUsers");
        }
    }
}
