using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAdministracionPeluqueria.Migrations
{
    /// <inheritdoc />
    public partial class Agregando_FK_IdUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdAdministrador",
                table: "Calendarios");

            migrationBuilder.AddColumn<string>(
                name: "IdUsuario",
                table: "Razas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Razas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdUsuario",
                table: "Enfermedades",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Enfermedades",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdUsuario",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Clientes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdUsuario",
                table: "Calendarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Calendarios",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdUsuario",
                table: "Alergias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Alergias",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Razas_UsuarioId",
                table: "Razas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Enfermedades_UsuarioId",
                table: "Enfermedades",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_UsuarioId",
                table: "Clientes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Calendarios_UsuarioId",
                table: "Calendarios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Alergias_UsuarioId",
                table: "Alergias",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alergias_AspNetUsers_UsuarioId",
                table: "Alergias",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Calendarios_AspNetUsers_UsuarioId",
                table: "Calendarios",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clientes_AspNetUsers_UsuarioId",
                table: "Clientes",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Enfermedades_AspNetUsers_UsuarioId",
                table: "Enfermedades",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Razas_AspNetUsers_UsuarioId",
                table: "Razas",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alergias_AspNetUsers_UsuarioId",
                table: "Alergias");

            migrationBuilder.DropForeignKey(
                name: "FK_Calendarios_AspNetUsers_UsuarioId",
                table: "Calendarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Clientes_AspNetUsers_UsuarioId",
                table: "Clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_Enfermedades_AspNetUsers_UsuarioId",
                table: "Enfermedades");

            migrationBuilder.DropForeignKey(
                name: "FK_Razas_AspNetUsers_UsuarioId",
                table: "Razas");

            migrationBuilder.DropIndex(
                name: "IX_Razas_UsuarioId",
                table: "Razas");

            migrationBuilder.DropIndex(
                name: "IX_Enfermedades_UsuarioId",
                table: "Enfermedades");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_UsuarioId",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Calendarios_UsuarioId",
                table: "Calendarios");

            migrationBuilder.DropIndex(
                name: "IX_Alergias_UsuarioId",
                table: "Alergias");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Razas");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Razas");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Enfermedades");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Enfermedades");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Calendarios");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Calendarios");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Alergias");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Alergias");

            migrationBuilder.AddColumn<int>(
                name: "IdAdministrador",
                table: "Calendarios",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
