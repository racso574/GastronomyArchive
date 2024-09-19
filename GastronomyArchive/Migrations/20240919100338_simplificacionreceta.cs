using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GastronomyArchive.Migrations
{
    /// <inheritdoc />
    public partial class simplificacionreceta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecetaAlimentos_Alimentos_AlimentoId",
                table: "RecetaAlimentos");

            migrationBuilder.DropIndex(
                name: "IX_RecetaAlimentos_AlimentoId",
                table: "RecetaAlimentos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RecetaAlimentos_AlimentoId",
                table: "RecetaAlimentos",
                column: "AlimentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecetaAlimentos_Alimentos_AlimentoId",
                table: "RecetaAlimentos",
                column: "AlimentoId",
                principalTable: "Alimentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
