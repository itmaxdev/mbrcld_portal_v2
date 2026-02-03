using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatData.Migrations
{
    public partial class addDescriptionAndImageInRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Time",
                value: new DateTime(2021, 10, 18, 16, 20, 50, 782, DateTimeKind.Utc).AddTicks(4111));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Time",
                value: new DateTime(2021, 10, 18, 16, 20, 50, 782, DateTimeKind.Utc).AddTicks(6109));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "Time",
                value: new DateTime(2021, 10, 18, 16, 20, 50, 782, DateTimeKind.Utc).AddTicks(6174));

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "Description", "Image" },
                values: new object[] { "Room 1 description", "" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "Description", "Image" },
                values: new object[] { "Room 2 description", "" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "Description", "Image" },
                values: new object[] { "Room 3 description", "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Rooms");

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Time",
                value: new DateTime(2021, 3, 1, 14, 58, 15, 588, DateTimeKind.Utc).AddTicks(1215));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Time",
                value: new DateTime(2021, 3, 1, 14, 58, 15, 588, DateTimeKind.Utc).AddTicks(4183));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "Time",
                value: new DateTime(2021, 3, 1, 14, 58, 15, 588, DateTimeKind.Utc).AddTicks(4244));
        }
    }
}
