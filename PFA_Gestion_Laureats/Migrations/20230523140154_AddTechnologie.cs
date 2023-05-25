using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PFA_Gestion_Laureats.Migrations
{
    /// <inheritdoc />
    public partial class AddTechnologie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ConnectionId",
                table: "Utilisateurs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "IsVisibleTel",
                table: "Utilisateurs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Nature",
                table: "Annonces",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Remuniration",
                table: "Annonces",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Technologie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technologie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnnonceTechnologie",
                columns: table => new
                {
                    annoncesId = table.Column<int>(type: "int", nullable: false),
                    technologiesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnonceTechnologie", x => new { x.annoncesId, x.technologiesId });
                    table.ForeignKey(
                        name: "FK_AnnonceTechnologie_Annonces_annoncesId",
                        column: x => x.annoncesId,
                        principalTable: "Annonces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnonceTechnologie_Technologie_technologiesId",
                        column: x => x.technologiesId,
                        principalTable: "Technologie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnonceTechnologie_technologiesId",
                table: "AnnonceTechnologie",
                column: "technologiesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnonceTechnologie");

            migrationBuilder.DropTable(
                name: "Technologie");

            migrationBuilder.DropColumn(
                name: "IsVisibleTel",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "Nature",
                table: "Annonces");

            migrationBuilder.DropColumn(
                name: "Remuniration",
                table: "Annonces");

            migrationBuilder.AlterColumn<string>(
                name: "ConnectionId",
                table: "Utilisateurs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
