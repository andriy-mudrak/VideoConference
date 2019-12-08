using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerTest.Migrations
{
    public partial class ChangeRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1c43423e-e95e-4213-866f-1bc048dcbd91");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9871aa9d-0b38-41c4-bab7-f4a2d04f5a81");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cd59a55c-3241-43de-9a85-8227c4e89200");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "220397bf-31e5-4705-810f-6087378d9428", "57c0a78e-24d1-4c3a-8b27-e7f9c37b47b3", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "2de5e83f-f111-4580-b95d-6651768a3d6f", "1f3c891a-f932-483e-add8-0aa4a46a73cc", "PremiumUser", "PREMIUMUSER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "40e64bc3-59d4-4ce3-9d6c-9b5430002df9", "f644acf2-202c-4244-9612-b206bfb278e1", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "220397bf-31e5-4705-810f-6087378d9428");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2de5e83f-f111-4580-b95d-6651768a3d6f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40e64bc3-59d4-4ce3-9d6c-9b5430002df9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cd59a55c-3241-43de-9a85-8227c4e89200", "01486768-4288-47bb-a0d0-580e40bf1788", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9871aa9d-0b38-41c4-bab7-f4a2d04f5a81", "71fde766-d947-4306-8ebb-5994bd4e3ae1", "PremiumUser", "PremiumUser" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1c43423e-e95e-4213-866f-1bc048dcbd91", "12805dd6-e194-4373-ab02-728a876dacfd", "Admin", "ADMIN" });
        }
    }
}
