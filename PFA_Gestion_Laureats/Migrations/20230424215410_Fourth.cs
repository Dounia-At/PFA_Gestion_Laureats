using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PFA_Gestion_Laureats.Migrations
{
    /// <inheritdoc />
    public partial class Fourth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sujet",
                table: "Stages",
                newName: "Intitulé_poste");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Expiration",
                table: "Certificat",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Intitulé_poste",
                table: "Stages",
                newName: "Sujet");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_Expiration",
                table: "Certificat",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
