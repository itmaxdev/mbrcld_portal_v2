using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatData.Migrations
{
    public partial class ReadableMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReaded",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Time",
                value: new DateTime(2021, 2, 24, 18, 3, 22, 686, DateTimeKind.Utc).AddTicks(4145));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Time",
                value: new DateTime(2021, 2, 24, 18, 3, 22, 686, DateTimeKind.Utc).AddTicks(7009));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "Time",
                value: new DateTime(2021, 2, 24, 18, 3, 22, 686, DateTimeKind.Utc).AddTicks(7078));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReaded",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Time",
                value: new DateTime(2021, 2, 23, 22, 6, 51, 0, DateTimeKind.Utc).AddTicks(5564));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Time",
                value: new DateTime(2021, 2, 23, 22, 6, 51, 0, DateTimeKind.Utc).AddTicks(8198));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "Time",
                value: new DateTime(2021, 2, 23, 22, 6, 51, 0, DateTimeKind.Utc).AddTicks(8272));
        }
    }
}
