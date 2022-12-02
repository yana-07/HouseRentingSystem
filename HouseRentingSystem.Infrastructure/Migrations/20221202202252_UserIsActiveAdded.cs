using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Infrastructure.Migrations
{
    public partial class UserIsActiveAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                columns: new[] { "ConcurrencyStamp", "IsActive", "PasswordHash", "SecurityStamp" },
                values: new object[] { "690bc3bf-59e5-47b3-bf62-75d1a17d1308", true, "AQAAAAEAACcQAAAAEFl5RcqWg/CBOfOtYaSFy/1XrQLDxHs93uj+Cphw3W59Wt+QWT6fa7JiKZyyOMoKsg==", "bb14a453-2dc4-48f8-b87f-56eb496fc0df" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dea12856-c198-4129-b3f3-b893d8395082",
                columns: new[] { "ConcurrencyStamp", "IsActive", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4bc9fcf5-c988-46d4-acf2-8c87bf393c65", true, "AQAAAAEAACcQAAAAEGNieXJIar+NMG660OHr7/aI9MOV+qiJbBknwk044o3KSjuPyekBqMsdoN6vyTis9A==", "51c93bb9-a202-4e42-a780-1b9c01eaf292" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

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
    }
}
