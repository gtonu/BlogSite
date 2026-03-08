using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DevSkill.Blog.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserRoleUserRoleCategoryTagSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("23fe4e81-5015-42a0-8d76-d1f08c6b227a"), "23fe4e81-5015-42a0-8d76-d1f08c6b227a", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CountryDialCode", "CountryName", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RegistrationDate", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("605fbeb7-f2bc-4d2c-886d-08de6c1b8c01"), 0, "bfb063fb-c852-4248-bd51-6c8e56f3e1bf", null, null, "admin@gmail.com", true, false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEFF00oytfTeMeV0LotTISYwoFEu+ngUmsBx4nHUnRh7xSdc1e9aVj6p4VGmRY4HWZg==", null, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "GDOC34LIOZXWECAJBPLLACWMF2CBR6GH", false, "Admin" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CategoryName" },
                values: new object[,]
                {
                    { new Guid("31be253f-245c-4653-8bbc-f3fa46c14071"), "JavaScript" },
                    { new Guid("6e9532ad-9883-4fc3-a0ed-992683c179d8"), "Networking" },
                    { new Guid("9d9b8e74-d010-432f-9203-5eed53f7b611"), "Software engineering" },
                    { new Guid("ab05a84d-ffe6-4125-81c4-1ced18cf95e1"), "Web Development" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "TagName" },
                values: new object[,]
                {
                    { new Guid("03f0eb39-ba69-4a83-9e3a-9606cdfb7104"), "asp.net" },
                    { new Guid("c8560ef7-c037-4b2f-8ce9-b82fc89eb7d4"), "Ajax" },
                    { new Guid("d478015c-b35b-41c7-8dde-07699be0f1d3"), "mvc" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("23fe4e81-5015-42a0-8d76-d1f08c6b227a"), new Guid("605fbeb7-f2bc-4d2c-886d-08de6c1b8c01") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("23fe4e81-5015-42a0-8d76-d1f08c6b227a"), new Guid("605fbeb7-f2bc-4d2c-886d-08de6c1b8c01") });

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("31be253f-245c-4653-8bbc-f3fa46c14071"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("6e9532ad-9883-4fc3-a0ed-992683c179d8"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("9d9b8e74-d010-432f-9203-5eed53f7b611"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("ab05a84d-ffe6-4125-81c4-1ced18cf95e1"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("03f0eb39-ba69-4a83-9e3a-9606cdfb7104"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("c8560ef7-c037-4b2f-8ce9-b82fc89eb7d4"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("d478015c-b35b-41c7-8dde-07699be0f1d3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("23fe4e81-5015-42a0-8d76-d1f08c6b227a"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("605fbeb7-f2bc-4d2c-886d-08de6c1b8c01"));
        }
    }
}
