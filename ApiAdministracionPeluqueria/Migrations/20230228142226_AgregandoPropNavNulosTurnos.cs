using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAdministracionPeluqueria.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoPropNavNulosTurnos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Fechas_FechaId",
                table: "Turnos");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Horarios_HorarioId",
                table: "Turnos");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Mascotas_MascotaId",
                table: "Turnos");

            migrationBuilder.AlterColumn<int>(
                name: "MascotaId",
                table: "Turnos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "HorarioId",
                table: "Turnos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FechaId",
                table: "Turnos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Fechas_FechaId",
                table: "Turnos",
                column: "FechaId",
                principalTable: "Fechas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Horarios_HorarioId",
                table: "Turnos",
                column: "HorarioId",
                principalTable: "Horarios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Mascotas_MascotaId",
                table: "Turnos",
                column: "MascotaId",
                principalTable: "Mascotas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Fechas_FechaId",
                table: "Turnos");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Horarios_HorarioId",
                table: "Turnos");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Mascotas_MascotaId",
                table: "Turnos");

            migrationBuilder.AlterColumn<int>(
                name: "MascotaId",
                table: "Turnos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HorarioId",
                table: "Turnos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FechaId",
                table: "Turnos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Fechas_FechaId",
                table: "Turnos",
                column: "FechaId",
                principalTable: "Fechas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Horarios_HorarioId",
                table: "Turnos",
                column: "HorarioId",
                principalTable: "Horarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Mascotas_MascotaId",
                table: "Turnos",
                column: "MascotaId",
                principalTable: "Mascotas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
