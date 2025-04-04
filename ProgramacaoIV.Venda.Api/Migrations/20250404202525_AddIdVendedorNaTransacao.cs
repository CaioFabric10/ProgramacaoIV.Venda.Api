using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProgramacaoIV.Venda.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIdVendedorNaTransacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ID_VENDEDOR",
                table: "TRANSACAO",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TRANSACAO_ID_VENDEDOR",
                table: "TRANSACAO",
                column: "ID_VENDEDOR");

            migrationBuilder.AddForeignKey(
                name: "FK_TRANSACAO_Vendedor_ID_VENDEDOR",
                table: "TRANSACAO",
                column: "ID_VENDEDOR",
                principalTable: "Vendedor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TRANSACAO_Vendedor_ID_VENDEDOR",
                table: "TRANSACAO");

            migrationBuilder.DropIndex(
                name: "IX_TRANSACAO_ID_VENDEDOR",
                table: "TRANSACAO");

            migrationBuilder.DropColumn(
                name: "ID_VENDEDOR",
                table: "TRANSACAO");
        }
    }
}
