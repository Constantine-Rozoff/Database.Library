using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Library.Entity.Migrations
{
    /// <inheritdoc />
    public partial class MakeReaderIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Librarians_Readers_ReaderId",
                table: "Librarians");

            migrationBuilder.AlterColumn<int>(
                name: "ReaderId",
                table: "Librarians",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Librarians_Readers_ReaderId",
                table: "Librarians",
                column: "ReaderId",
                principalTable: "Readers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Librarians_Readers_ReaderId",
                table: "Librarians");

            migrationBuilder.AlterColumn<int>(
                name: "ReaderId",
                table: "Librarians",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Librarians_Readers_ReaderId",
                table: "Librarians",
                column: "ReaderId",
                principalTable: "Readers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
