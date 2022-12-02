using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Infrastructure.Migrations
{
    public partial class UserExtended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "208c3cc6-6e31-4ef2-b0d3-55d9145d8383", "AQAAAAEAACcQAAAAEDZJRWZQnwhQ5mC4GbF/5N2DrYIjJFq4p401bRGVfsLfLG4WotevnG+TE/Byg9UCyw==", "ef3cc7e7-5ba8-498c-aae2-46e06a893cc3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f8ee8104-68af-454e-b71c-793fefccd620", "AQAAAAEAACcQAAAAEGptBiGd7/RCSHdyncna6ojBQJOZY8Qqh0BBxFB0dVCjF+GQlMvs9T6+o3zSLG/RDg==", "05819a88-dd5d-479b-9289-e794cd4c444c" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f57852f6-0892-415c-8078-40be02c0acc0", "AQAAAAEAACcQAAAAEPtvK6eNyDi9J6dJRtMZU3PH1r/g3Xcc9EFkDLakTH5NcHuGN5WJKUnJ2xWYxaE1kA==", "ca5bdf78-6714-457f-9df4-077204717a85" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8d528d55-30bf-492c-a52f-6c81e0aa43d6", "AQAAAAEAACcQAAAAEDAk0UJVi+gxXIc46aDhb+5jmowf1xR8oltRgwBdB3/v4B8PZ/Gr759jcs7hmN5fgA==", "436913f5-a9d5-430c-8a74-48e25a447b51" });
        }
    }
}
