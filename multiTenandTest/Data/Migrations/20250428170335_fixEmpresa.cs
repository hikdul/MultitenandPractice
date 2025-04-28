using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace multiTenandTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixEmpresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empresa_AspNetUsers_userCreatedIdId",
                table: "Empresa");

            migrationBuilder.DropIndex(
                name: "IX_Empresa_userCreatedIdId",
                table: "Empresa");

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: "2f79242c-c0bd-4e2e-8b7c-d97686a8bf5d");

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: "4696ef4f-67f3-4062-a9ca-a2a8bc31df3a");

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: "6e36ddcf-164b-4827-8037-d8c9cfec6dee");

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: "749a561a-6092-44e8-9f29-aef467a18d81");

            migrationBuilder.DropColumn(
                name: "userCreated",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "userCreatedIdId",
                table: "Empresa");

            migrationBuilder.AddColumn<string>(
                name: "userCreatedId",
                table: "Empresa",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Paises",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "143b6259-4dcf-45c7-ac78-c9888e1bd4fd", "México" },
                    { "6d8f3e51-a9d9-429d-91fb-b23210041560", "Colombia" },
                    { "df2cd098-6854-4d20-b3cb-d67910428a4d", "Venezuela" },
                    { "fcc1835e-8a7c-49d6-bb9f-d6eb7f8d3d6a", "República Dominicana" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_userCreatedId",
                table: "Empresa",
                column: "userCreatedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empresa_AspNetUsers_userCreatedId",
                table: "Empresa",
                column: "userCreatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empresa_AspNetUsers_userCreatedId",
                table: "Empresa");

            migrationBuilder.DropIndex(
                name: "IX_Empresa_userCreatedId",
                table: "Empresa");

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: "143b6259-4dcf-45c7-ac78-c9888e1bd4fd");

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: "6d8f3e51-a9d9-429d-91fb-b23210041560");

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: "df2cd098-6854-4d20-b3cb-d67910428a4d");

            migrationBuilder.DeleteData(
                table: "Paises",
                keyColumn: "Id",
                keyValue: "fcc1835e-8a7c-49d6-bb9f-d6eb7f8d3d6a");

            migrationBuilder.DropColumn(
                name: "userCreatedId",
                table: "Empresa");

            migrationBuilder.AddColumn<string>(
                name: "userCreated",
                table: "Empresa",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userCreatedIdId",
                table: "Empresa",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Paises",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { "2f79242c-c0bd-4e2e-8b7c-d97686a8bf5d", "Venezuela" },
                    { "4696ef4f-67f3-4062-a9ca-a2a8bc31df3a", "República Dominicana" },
                    { "6e36ddcf-164b-4827-8037-d8c9cfec6dee", "México" },
                    { "749a561a-6092-44e8-9f29-aef467a18d81", "Colombia" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_userCreatedIdId",
                table: "Empresa",
                column: "userCreatedIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empresa_AspNetUsers_userCreatedIdId",
                table: "Empresa",
                column: "userCreatedIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
