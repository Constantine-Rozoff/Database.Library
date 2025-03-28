using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Library.Entity.Migrations
{
    /// <inheritdoc />
    public partial class BookAuthorTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "BookAuthors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "BookAuthors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "BookAuthors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "BookAuthors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "BookAuthors");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "BookAuthors");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "BookAuthors");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "BookAuthors");
        }
    }
}
