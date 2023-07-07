using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAdministracionPeluqueria.Migrations
{
    /// <inheritdoc />
    public partial class agregacionEntidadCaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CalendarioId",
                table: "Turnos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Caja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    IdUsuario = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Caja_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_CalendarioId",
                table: "Turnos",
                column: "CalendarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Caja_UsuarioId",
                table: "Caja",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Turnos_Calendarios_CalendarioId",
                table: "Turnos",
                column: "CalendarioId",
                principalTable: "Calendarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Turnos_Calendarios_CalendarioId",
                table: "Turnos");

            migrationBuilder.DropTable(
                name: "Caja");

            migrationBuilder.DropIndex(
                name: "IX_Turnos_CalendarioId",
                table: "Turnos");

            migrationBuilder.DropColumn(
                name: "CalendarioId",
                table: "Turnos");
        }
    }
}
