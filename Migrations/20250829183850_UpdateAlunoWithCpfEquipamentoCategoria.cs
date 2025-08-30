using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademiaMvc.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAlunoWithCpfEquipamentoCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "Alunos",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "Alunos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EquipamentoId",
                table: "Alunos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_CategoriaId",
                table: "Alunos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_EquipamentoId",
                table: "Alunos",
                column: "EquipamentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alunos_Categorias_CategoriaId",
                table: "Alunos",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Alunos_Equipamentos_EquipamentoId",
                table: "Alunos",
                column: "EquipamentoId",
                principalTable: "Equipamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alunos_Categorias_CategoriaId",
                table: "Alunos");

            migrationBuilder.DropForeignKey(
                name: "FK_Alunos_Equipamentos_EquipamentoId",
                table: "Alunos");

            migrationBuilder.DropIndex(
                name: "IX_Alunos_CategoriaId",
                table: "Alunos");

            migrationBuilder.DropIndex(
                name: "IX_Alunos_EquipamentoId",
                table: "Alunos");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Alunos");

            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "Alunos");

            migrationBuilder.DropColumn(
                name: "EquipamentoId",
                table: "Alunos");
        }
    }
}
