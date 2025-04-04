using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramacaoIV.Venda.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitComVendedor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vendedor",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false),
                    NM_VENDEDOR = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    DS_EMAIL = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    IN_ATIVO = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DT_CRIACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DT_ATUALIZACAO = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendedor", x => x.ID);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vendedor");
        }
    }
}
