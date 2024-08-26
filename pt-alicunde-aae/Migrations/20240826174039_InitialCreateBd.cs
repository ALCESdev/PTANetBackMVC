using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pt_alicunde_aae.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateBd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EntidadesEjemplo",
                table: "EntidadesEjemplo");

            migrationBuilder.RenameTable(
                name: "EntidadesEjemplo",
                newName: "Bank");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bank",
                table: "Bank",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Bank",
                table: "Bank");

            migrationBuilder.RenameTable(
                name: "Bank",
                newName: "EntidadesEjemplo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntidadesEjemplo",
                table: "EntidadesEjemplo",
                column: "id");
        }
    }
}
