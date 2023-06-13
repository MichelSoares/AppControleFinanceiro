using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleFinanceiroAPI.Migrations
{
    /// <inheritdoc />
    public partial class RelacaoUserTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "user_credential_id",
                table: "transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_transaction_user_credential_id",
                table: "transaction",
                column: "user_credential_id");

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_user_credential_user_credential_id",
                table: "transaction",
                column: "user_credential_id",
                principalTable: "user_credential",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transaction_user_credential_user_credential_id",
                table: "transaction");

            migrationBuilder.DropIndex(
                name: "IX_transaction_user_credential_id",
                table: "transaction");

            migrationBuilder.DropColumn(
                name: "user_credential_id",
                table: "transaction");
        }
    }
}
