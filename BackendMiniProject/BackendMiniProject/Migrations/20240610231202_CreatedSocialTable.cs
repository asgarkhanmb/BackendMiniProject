using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendMiniProject.Migrations
{
    public partial class CreatedSocialTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstructorSocials_Social_SocialId",
                table: "InstructorSocials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Social",
                table: "Social");

            migrationBuilder.RenameTable(
                name: "Social",
                newName: "Socials");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Socials",
                table: "Socials",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Profession = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoftDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorSocials_Socials_SocialId",
                table: "InstructorSocials",
                column: "SocialId",
                principalTable: "Socials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InstructorSocials_Socials_SocialId",
                table: "InstructorSocials");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Socials",
                table: "Socials");

            migrationBuilder.RenameTable(
                name: "Socials",
                newName: "Social");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Social",
                table: "Social",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InstructorSocials_Social_SocialId",
                table: "InstructorSocials",
                column: "SocialId",
                principalTable: "Social",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
