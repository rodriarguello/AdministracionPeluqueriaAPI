using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAdministracionPeluqueria.Migrations
{
    public partial class modificacionTurnosMascotas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Mascotas_MascotaId",
                table: "Turnos");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Mascotas_MascotaId",
                table: "Turnos",
                column: "MascotaId",
                principalTable: "Mascotas",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Mascotas_MascotaId",
                table: "Turnos");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Mascotas_MascotaId",
                table: "Turnos",
                column: "MascotaId",
                principalTable: "Mascotas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
