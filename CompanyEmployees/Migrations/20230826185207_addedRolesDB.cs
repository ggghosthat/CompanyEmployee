using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CompanyEmployees.Migrations
{
    /// <inheritdoc />
    public partial class addedRolesDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "ac9a70a0-a1d4-42b5-954c-8f1e18e49534", "9dffb713-a09b-4d98-9c3f-29012586a704", "Administrator", "ADMINISTRATOR" },
                    { "f355c520-d207-40d1-9214-ff4730bee334", "cbc6b725-1021-4611-8d96-6cb54a8a3b0c", "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac9a70a0-a1d4-42b5-954c-8f1e18e49534");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f355c520-d207-40d1-9214-ff4730bee334");
        }
    }
}
