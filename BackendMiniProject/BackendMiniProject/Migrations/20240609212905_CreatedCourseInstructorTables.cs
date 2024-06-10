using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendMiniProject.Migrations
{
    public partial class CreatedCourseInstructorTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Instructor_InstructorId",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Instructor",
                table: "Instructor");

            migrationBuilder.RenameTable(
                name: "Instructor",
                newName: "Instructors");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Instructors",
                table: "Instructors",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Social",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoftDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Social", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstructorSocials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InstructorId = table.Column<int>(type: "int", nullable: false),
                    SocialId = table.Column<int>(type: "int", nullable: false),
                    SocialLink = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorSocials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstructorSocials_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorSocials_Social_SocialId",
                        column: x => x.SocialId,
                        principalTable: "Social",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstructorSocials_InstructorId",
                table: "InstructorSocials",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorSocials_SocialId",
                table: "InstructorSocials",
                column: "SocialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Instructors_InstructorId",
                table: "Courses",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Instructors_InstructorId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "InstructorSocials");

            migrationBuilder.DropTable(
                name: "Social");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Instructors",
                table: "Instructors");

            migrationBuilder.RenameTable(
                name: "Instructors",
                newName: "Instructor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Instructor",
                table: "Instructor",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Instructor_InstructorId",
                table: "Courses",
                column: "InstructorId",
                principalTable: "Instructor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
