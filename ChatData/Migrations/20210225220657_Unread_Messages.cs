using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatData.Migrations
{
    public partial class Unread_Messages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReaded",
                table: "Messages");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMessageDate",
                table: "Participants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "LastMessageId",
                table: "Participants",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "MessageType",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Time",
                value: new DateTime(2021, 2, 25, 22, 6, 56, 629, DateTimeKind.Utc).AddTicks(8864));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Time",
                value: new DateTime(2021, 2, 25, 22, 6, 56, 630, DateTimeKind.Utc).AddTicks(3394));

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "Time",
                value: new DateTime(2021, 2, 25, 22, 6, 56, 630, DateTimeKind.Utc).AddTicks(3582));

            migrationBuilder.UpdateData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "LastMessageId",
                value: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.UpdateData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "LastMessageId",
                value: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.UpdateData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "LastMessageId",
                value: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.UpdateData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "LastMessageId",
                value: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.UpdateData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                column: "LastMessageId",
                value: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.UpdateData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                column: "LastMessageId",
                value: new Guid("33333333-3333-3333-3333-333333333333"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMessageDate",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "LastMessageId",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "MessageType",
                table: "Messages");

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
    }
}
