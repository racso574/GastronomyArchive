using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GastronomyArchive.Migrations
{
    /// <inheritdoc />
    public partial class AddCantidadPersonasBaseToReceta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CantidadPersonasBase",
                table: "Recetas",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadPersonasBase",
                table: "Recetas");
        }
    }
}
