using Microsoft.EntityFrameworkCore.Migrations;

namespace WebTask2.Data.Migrations
{
    public partial class server : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServerNumber",
                table: "Requests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServerNumber",
                table: "Requests");
        }
    }
}
