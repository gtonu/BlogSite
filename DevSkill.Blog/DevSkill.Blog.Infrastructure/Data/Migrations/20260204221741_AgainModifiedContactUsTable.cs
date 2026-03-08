using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Blog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgainModifiedContactUsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ContactUs",
                newName: "SenderName");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "ContactUs",
                newName: "SenderEmail");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "ContactUs",
                newName: "ReceivedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "RepliedDate",
                table: "ContactUs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reply",
                table: "ContactUs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepliedDate",
                table: "ContactUs");

            migrationBuilder.DropColumn(
                name: "Reply",
                table: "ContactUs");

            migrationBuilder.RenameColumn(
                name: "SenderName",
                table: "ContactUs",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SenderEmail",
                table: "ContactUs",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "ReceivedDate",
                table: "ContactUs",
                newName: "Date");
        }
    }
}
