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
            migrationBuilder.AddColumn<bool>(
                name: "IsComfirmed",
                table: "Utilisateurs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "date_Login",
                table: "Utilisateurs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "date_Logout",
                table: "Utilisateurs",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComfirmed",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "date_Login",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "date_Logout",
                table: "Utilisateurs");
        }
    }
}
