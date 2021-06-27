using Microsoft.EntityFrameworkCore.Migrations;

namespace ListaDeFilmes.Data.Migrations
{
    public partial class NovosCamposFilmes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Tb_Filmes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "Tb_Filmes",
                type: "varchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Valor",
                table: "Tb_Filmes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Tb_Filmes");

            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Tb_Filmes");

            migrationBuilder.DropColumn(
                name: "Valor",
                table: "Tb_Filmes");
        }
    }
}
