using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAdministracionPeluqueria.Migrations
{
    /// <inheritdoc />
    public partial class CreacionEntidadCalendario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calendarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraInicioTurnos = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraFinTurnos = table.Column<TimeSpan>(type: "time", nullable: false),
                    IntervaloTurnos = table.Column<TimeSpan>(type: "time", nullable: false),
                    IdAdministrador = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendarios", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calendarios");
        }
    }
}
