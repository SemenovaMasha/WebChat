using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalR_Identity.Migrations
{
    public partial class userProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatingDate",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CropAvatarId",
                table: "AspNetUsers",
                nullable: true);
            
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CropAvatarId",
                table: "AspNetUsers",
                column: "CropAvatarId");
            
            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Image_CropAvatarId",
                table: "AspNetUsers",
                column: "CropAvatarId",
                principalTable: "Image",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Image_CropAvatarId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CropAvatarId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatingDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CropAvatarId",
                table: "AspNetUsers");
            
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");
        }
    }
}
