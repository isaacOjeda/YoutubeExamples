using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFirstApi.Migrations
{
    public partial class AddedPhotoName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoName",
                table: "Products");
        }
    }
}
