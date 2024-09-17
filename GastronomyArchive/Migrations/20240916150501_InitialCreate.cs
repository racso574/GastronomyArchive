using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GastronomyArchive.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaloriesPer100g = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProteinsPer100g = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FatsPer100g = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CarbsPer100g = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AverageWeightGrams = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Foods");
        }
    }
}
