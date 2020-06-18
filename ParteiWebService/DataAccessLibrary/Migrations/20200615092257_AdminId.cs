using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLibrary.Migrations
{
    public partial class AdminId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Organizations_AdminId",
                table: "Organizations");

            migrationBuilder.AddColumn<int>(
                name: "OrgranizationId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_AdminId",
                table: "Organizations",
                column: "AdminId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Organizations_AdminId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "OrgranizationId",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_AdminId",
                table: "Organizations",
                column: "AdminId");
        }
    }
}
