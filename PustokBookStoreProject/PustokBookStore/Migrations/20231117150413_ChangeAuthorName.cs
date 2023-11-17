using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PustokBookStore.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAuthorName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_AuthorId_AuthorId",
                table: "Book");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorId",
                table: "AuthorId");

            migrationBuilder.RenameTable(
                name: "AuthorId",
                newName: "Author");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Author",
                table: "Author",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Author_AuthorId",
                table: "Book",
                column: "AuthorId",
                principalTable: "Author",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Author_AuthorId",
                table: "Book");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Author",
                table: "Author");

            migrationBuilder.RenameTable(
                name: "Author",
                newName: "AuthorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorId",
                table: "AuthorId",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_AuthorId_AuthorId",
                table: "Book",
                column: "AuthorId",
                principalTable: "AuthorId",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
