using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ControleFinanceiroAPI.Migrations
{
    /// <inheritdoc />
    public partial class PopulaTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO public.transaction(type, name, date, value, user_credential_id) VALUES (1, 'salário', '2023-05-28', '100000.00', 1);");
            mb.Sql("INSERT INTO public.transaction(type, name, date, value, user_credential_id) VALUES (1, 'free lance', '2023-05-28', '4500.00', 1);");
            mb.Sql("INSERT INTO public.transaction(type, name, date, value, user_credential_id) VALUES (2, 'Alienware M15 R7', '2023-05-28', '11500.00', 1);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM transaction;");
        }
    }
}
