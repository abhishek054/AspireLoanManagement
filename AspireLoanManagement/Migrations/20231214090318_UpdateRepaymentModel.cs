using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspireLoanManagement.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRepaymentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RepaymentAmount",
                table: "Repayments",
                newName: "PendingAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "Repayments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "Repayments");

            migrationBuilder.RenameColumn(
                name: "PendingAmount",
                table: "Repayments",
                newName: "RepaymentAmount");
        }
    }
}
