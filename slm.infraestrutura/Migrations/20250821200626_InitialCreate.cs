using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace slm.infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "entregadores",
                columns: table => new
                {
                    identificador = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    data_nascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    numero_cnh = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    tipo_cnh = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    imagem_cnh = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entregadores", x => x.identificador);
                });

            migrationBuilder.CreateTable(
                name: "locacao",
                columns: table => new
                {
                    identificador = table.Column<string>(type: "text", nullable: false),
                    valor_diaria = table.Column<decimal>(type: "numeric", nullable: false),
                    valor_total = table.Column<decimal>(type: "numeric", nullable: true),
                    entregador_id = table.Column<string>(type: "text", nullable: false),
                    moto_id = table.Column<string>(type: "text", nullable: false),
                    data_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_termino = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_previsao_termino = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    plano = table.Column<int>(type: "integer", nullable: false),
                    data_devolucao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locacao", x => x.identificador);
                });

            migrationBuilder.CreateTable(
                name: "motos",
                columns: table => new
                {
                    identificador = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ano = table.Column<int>(type: "integer", nullable: false),
                    modelo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    placa = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_motos", x => x.identificador);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "entregadores");

            migrationBuilder.DropTable(
                name: "locacao");

            migrationBuilder.DropTable(
                name: "motos");
        }
    }
}
