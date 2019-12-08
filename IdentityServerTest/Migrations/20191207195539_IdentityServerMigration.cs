using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServerTest.Migrations
{
    public partial class IdentityServerMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0b999138-e585-4d86-a96a-e35629814517");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ea39bfe-6072-4adf-96f4-6c1687eaadd7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c211ac2c-9d1a-495b-b212-178f86d0972c");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { "7ea39bfe-6072-4adf-96f4-6c1687eaadd7", "46aa22c0-00e5-41a5-846e-e21dc2527299", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0b999138-e585-4d86-a96a-e35629814517", "d3712c49-3c96-4b26-9595-16c1c285c8a2", "PremiumUser", "PremiumUser" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c211ac2c-9d1a-495b-b212-178f86d0972c", "fae636b1-ee94-4a01-9ef2-434d0586c987", "Admin", "ADMIN" });
        }
    }
}
