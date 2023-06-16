using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAdministracionPeluqueria.Migrations
{
    /// <inheritdoc />
    public partial class AgregacionpropNavegacionMascotaAlergia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mascotas_Alergias_AlergiaId",
                table: "Mascotas");

            migrationBuilder.DropIndex(
                name: "IX_Mascotas_AlergiaId",
                table: "Mascotas");

            migrationBuilder.DropColumn(
                name: "AlergiaId",
                table: "Mascotas");

            migrationBuilder.DropColumn(
                name: "IdAlergia",
                table: "Mascotas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AlergiaId",
                table: "Mascotas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdAlergia",
                table: "Mascotas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Mascotas_AlergiaId",
                table: "Mascotas",
                column: "AlergiaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mascotas_Alergias_AlergiaId",
                table: "Mascotas",
                column: "AlergiaId",
                principalTable: "Alergias",
                principalColumn: "Id");
        }
    }
}
