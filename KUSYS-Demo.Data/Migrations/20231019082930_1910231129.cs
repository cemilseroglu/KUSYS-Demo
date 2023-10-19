using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KUSYS_Demo.Data.Migrations
{
    public partial class _1910231129 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_AspNetUsers_StudentId",
                table: "CourseUser");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "CourseUser",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseUser_StudentId",
                table: "CourseUser",
                newName: "IX_CourseUser_UserId");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "AspNetUsers",
                newName: "StudentNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_AspNetUsers_UserId",
                table: "CourseUser",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseUser_AspNetUsers_UserId",
                table: "CourseUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CourseUser",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseUser_UserId",
                table: "CourseUser",
                newName: "IX_CourseUser_StudentId");

            migrationBuilder.RenameColumn(
                name: "StudentNumber",
                table: "AspNetUsers",
                newName: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseUser_AspNetUsers_StudentId",
                table: "CourseUser",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
