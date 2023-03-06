using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAdministracionPeluqueria.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionEntidadCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdMascota",
                table: "Clientes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdMascota",
                table: "Clientes",
                type: "int",
                nullable: true);
        }
    }
}
