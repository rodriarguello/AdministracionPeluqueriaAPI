using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAdministracionPeluqueria.Migrations
{
    /// <inheritdoc />
    public partial class RemoviendoEntidadMascota : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alergias_Mascotas_MascotaId",
                table: "Alergias");

            migrationBuilder.DropForeignKey(
                name: "FK_Enfermedades_Mascotas_MascotaId",
                table: "Enfermedades");

            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Mascotas_MascotaId",
                table: "Turnos");

            migrationBuilder.DropTable(
                name: "Mascotas");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_MascotaId",
                table: "Turnos");

            migrationBuilder.DropIndex(
                name: "IX_Enfermedades_MascotaId",
                table: "Enfermedades");

            migrationBuilder.DropIndex(
                name: "IX_Alergias_MascotaId",
                table: "Alergias");

            migrationBuilder.DropColumn(
                name: "MascotaId",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "MascotaId",
                table: "Enfermedades");

            migrationBuilder.DropColumn(
                name: "MascotaId",
                table: "Alergias");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MascotaId",
                table: "Turnos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MascotaId",
                table: "Enfermedades",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MascotaId",
                table: "Alergias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Mascotas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    RazaId = table.Column<int>(type: "int", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdAlergia = table.Column<int>(type: "int", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdEnfermedad = table.Column<int>(type: "int", nullable: false),
                    IdRaza = table.Column<int>(type: "int", nullable: false),
                    IdTurno = table.Column<int>(type: "int", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mascotas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mascotas_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mascotas_Razas_RazaId",
                        column: x => x.RazaId,
                        principalTable: "Razas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_MascotaId",
                table: "Turnos",
                column: "MascotaId");

            migrationBuilder.CreateIndex(
                name: "IX_Enfermedades_MascotaId",
                table: "Enfermedades",
                column: "MascotaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alergias_MascotaId",
                table: "Alergias",
                column: "MascotaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mascotas_ClienteId",
                table: "Mascotas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Mascotas_RazaId",
                table: "Mascotas",
                column: "RazaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alergias_Mascotas_MascotaId",
                table: "Alergias",
                column: "MascotaId",
                principalTable: "Mascotas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Enfermedades_Mascotas_MascotaId",
                table: "Enfermedades",
                column: "MascotaId",
                principalTable: "Mascotas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Mascotas_MascotaId",
                table: "Turnos",
                column: "MascotaId",
                principalTable: "Mascotas",
                principalColumn: "Id");
        }
    }
}
