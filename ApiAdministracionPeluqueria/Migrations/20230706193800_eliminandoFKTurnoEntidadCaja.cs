using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAdministracionPeluqueria.Migrations
{
    /// <inheritdoc />
    public partial class eliminandoFKTurnoEntidadCaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Caja_Turnos_TurnoId",
                table: "Caja");

            migrationBuilder.DropIndex(
                name: "IX_Caja_TurnoId",
                table: "Caja");

            migrationBuilder.DropColumn(
                name: "TurnoId",
                table: "Caja");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TurnoId",
                table: "Caja",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Caja_TurnoId",
                table: "Caja",
                column: "TurnoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Caja_Turnos_TurnoId",
                table: "Caja",
                column: "TurnoId",
                principalTable: "Turnos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
