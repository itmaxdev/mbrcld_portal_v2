using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatData.Migrations
{
    public partial class data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Name", "Type" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), "Room 1", "Type1" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Name", "Type" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), "Room 2", "Type2" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Name", "Type" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), "Room 3", "Type3" });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "RoomId", "Text", "Time", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111"), "Message1", new DateTime(2021, 2, 23, 22, 6, 51, 0, DateTimeKind.Utc).AddTicks(5564), new Guid("5e78f76e-3bb5-41b8-8df9-1b951c6570f7") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("22222222-2222-2222-2222-222222222222"), "Message2", new DateTime(2021, 2, 23, 22, 6, 51, 0, DateTimeKind.Utc).AddTicks(8198), new Guid("5e78f76e-3bb5-41b8-8df9-1b951c6570f7") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("33333333-3333-3333-3333-333333333333"), "Message3", new DateTime(2021, 2, 23, 22, 6, 51, 0, DateTimeKind.Utc).AddTicks(8272), new Guid("5e78f76e-3bb5-41b8-8df9-1b951c6570f7") }
                });

            migrationBuilder.InsertData(
                table: "Participants",
                columns: new[] { "Id", "RoomId", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("5e78f76e-3bb5-41b8-8df9-1b951c6570f7") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("e8854ae6-b79d-4d22-a680-e7be40c2c694") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("22222222-2222-2222-2222-222222222222"), new Guid("5e78f76e-3bb5-41b8-8df9-1b951c6570f7") },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new Guid("22222222-2222-2222-2222-222222222222"), new Guid("e8854ae6-b79d-4d22-a680-e7be40c2c694") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("5e78f76e-3bb5-41b8-8df9-1b951c6570f7") },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("e8854ae6-b79d-4d22-a680-e7be40c2c694") }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));

            migrationBuilder.DeleteData(
                table: "Participants",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));
        }
    }
}
