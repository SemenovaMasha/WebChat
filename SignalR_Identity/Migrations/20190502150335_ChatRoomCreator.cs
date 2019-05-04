using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalR_Identity.Migrations
{
    public partial class ChatRoomCreator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "ChatGroups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroups_CreatorId",
                table: "ChatGroups",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatGroups_AspNetUsers_CreatorId",
                table: "ChatGroups",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatGroups_AspNetUsers_CreatorId",
                table: "ChatGroups");

            migrationBuilder.DropIndex(
                name: "IX_ChatGroups_CreatorId",
                table: "ChatGroups");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ChatGroups");
        }
    }
}
