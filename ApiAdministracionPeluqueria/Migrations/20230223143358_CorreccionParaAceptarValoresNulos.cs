using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAdministracionPeluqueria.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionParaAceptarValoresNulos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mascotas_Razas_Razaid",
                table: "Mascotas");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "Razas",
                newName: "Nombre");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Razas",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "Razaid",
                table: "Mascotas",
                newName: "RazaId");

            migrationBuilder.RenameIndex(
                name: "IX_Mascotas_Razaid",
                table: "Mascotas",
                newName: "IX_Mascotas_RazaId");

            migrationBuilder.AlterColumn<int>(
                name: "IdTurno",
                table: "Mascotas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Mail",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Mascotas_Razas_RazaId",
                table: "Mascotas",
                column: "RazaId",
                principalTable: "Razas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mascotas_Razas_RazaId",
                table: "Mascotas");

            migrationBuilder.RenameColumn(
                name: "Nombre",
                table: "Razas",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Razas",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "RazaId",
                table: "Mascotas",
                newName: "Razaid");

            migrationBuilder.RenameIndex(
                name: "IX_Mascotas_RazaId",
                table: "Mascotas",
                newName: "IX_Mascotas_Razaid");

            migrationBuilder.AlterColumn<int>(
                name: "IdTurno",
                table: "Mascotas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Mail",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mascotas_Razas_Razaid",
                table: "Mascotas",
                column: "Razaid",
                principalTable: "Razas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
