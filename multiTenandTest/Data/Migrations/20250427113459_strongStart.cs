using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace multiTenandTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class strongStart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userCreated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    userCreatedIdId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empresa_AspNetUsers_userCreatedIdId",
                        column: x => x.userCreatedIdId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Paises",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmpresaUsuarioPermisos",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    permiso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpresaUsuarioPermisos", x => new { x.EmpresaId, x.UserId, x.permiso });
                    table.ForeignKey(
                        name: "FK_EmpresaUsuarioPermisos_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EmpresaUsuarioPermisos_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Vinculacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vinculacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vinculacion_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Vinculacion_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_EmpresaUsuarioPermisos_UserId",
                table: "EmpresaUsuarioPermisos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_TenantId",
                table: "Productos",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Vinculacion_EmpresaId",
                table: "Vinculacion",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Vinculacion_UserId",
                table: "Vinculacion",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpresaUsuarioPermisos");

            migrationBuilder.DropTable(
                name: "Paises");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Vinculacion");

            migrationBuilder.DropTable(
                name: "Empresa");
        }
    }
}
