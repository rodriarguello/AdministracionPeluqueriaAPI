using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAdministracionPeluqueria.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoEntidadMascotasAlergias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MascotaEnfermedades_Enfermedades_EnfermedadId",
                table: "MascotaEnfermedades");

            migrationBuilder.DropForeignKey(
                name: "FK_MascotaEnfermedades_Mascotas_MascotaId",
                table: "MascotaEnfermedades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MascotaEnfermedades",
                table: "MascotaEnfermedades");

            migrationBuilder.RenameTable(
                name: "MascotaEnfermedades",
                newName: "MascotasEnfermedades");

            migrationBuilder.RenameIndex(
                name: "IX_MascotaEnfermedades_EnfermedadId",
                table: "MascotasEnfermedades",
                newName: "IX_MascotasEnfermedades_EnfermedadId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MascotasEnfermedades",
                table: "MascotasEnfermedades",
                columns: new[] { "MascotaId", "EnfermedadId" });

            migrationBuilder.CreateTable(
                name: "MascotasAlergias",
                columns: table => new
                {
                    IdMascota = table.Column<int>(type: "int", nullable: false),
                    IdAlergia = table.Column<int>(type: "int", nullable: false),
                    MascotaId = table.Column<int>(type: "int", nullable: false),
                    AlergiaId = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MascotasAlergias", x => new { x.IdMascota, x.IdAlergia });
                    table.ForeignKey(
                        name: "FK_MascotasAlergias_Alergias_AlergiaId",
                        column: x => x.AlergiaId,
                        principalTable: "Alergias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MascotasAlergias_Mascotas_MascotaId",
                        column: x => x.MascotaId,
                        principalTable: "Mascotas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MascotasAlergias_AlergiaId",
                table: "MascotasAlergias",
                column: "AlergiaId");

            migrationBuilder.CreateIndex(
                name: "IX_MascotasAlergias_MascotaId",
                table: "MascotasAlergias",
                column: "MascotaId");

            migrationBuilder.AddForeignKey(
                name: "FK_MascotasEnfermedades_Enfermedades_EnfermedadId",
                table: "MascotasEnfermedades",
                column: "EnfermedadId",
                principalTable: "Enfermedades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MascotasEnfermedades_Mascotas_MascotaId",
                table: "MascotasEnfermedades",
                column: "MascotaId",
                principalTable: "Mascotas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MascotasEnfermedades_Enfermedades_EnfermedadId",
                table: "MascotasEnfermedades");

            migrationBuilder.DropForeignKey(
                name: "FK_MascotasEnfermedades_Mascotas_MascotaId",
                table: "MascotasEnfermedades");

            migrationBuilder.DropTable(
                name: "MascotasAlergias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MascotasEnfermedades",
                table: "MascotasEnfermedades");

            migrationBuilder.RenameTable(
                name: "MascotasEnfermedades",
                newName: "MascotaEnfermedades");

            migrationBuilder.RenameIndex(
                name: "IX_MascotasEnfermedades_EnfermedadId",
                table: "MascotaEnfermedades",
                newName: "IX_MascotaEnfermedades_EnfermedadId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MascotaEnfermedades",
                table: "MascotaEnfermedades",
                columns: new[] { "MascotaId", "EnfermedadId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MascotaEnfermedades_Enfermedades_EnfermedadId",
                table: "MascotaEnfermedades",
                column: "EnfermedadId",
                principalTable: "Enfermedades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MascotaEnfermedades_Mascotas_MascotaId",
                table: "MascotaEnfermedades",
                column: "MascotaId",
                principalTable: "Mascotas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
