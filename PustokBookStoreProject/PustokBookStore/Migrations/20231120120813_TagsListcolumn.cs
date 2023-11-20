using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PustokBookStore.Migrations
{
    /// <inheritdoc />
    public partial class TagsListcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Tags_TagsId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_TagsId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "TagsId",
                table: "Books");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagsId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_TagsId",
                table: "Books",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Tags_TagsId",
                table: "Books",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id");
        }
    }
}
