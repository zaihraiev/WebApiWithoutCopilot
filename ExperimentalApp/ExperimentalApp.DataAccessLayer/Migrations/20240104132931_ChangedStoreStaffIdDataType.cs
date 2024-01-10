using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExperimentalApp.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ChangedStoreStaffIdDataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_store_StoreId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "store_manager_staff_id_fkey",
                table: "store");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_StoreId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "manager_staff_id",
                table: "store",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "store_manager_staff_id_fkey",
                table: "store",
                column: "manager_staff_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "store_manager_staff_id_fkey",
                table: "store");

            migrationBuilder.AlterColumn<int>(
                name: "manager_staff_id",
                table: "store",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_StoreId",
                table: "AspNetUsers",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_store_StoreId",
                table: "AspNetUsers",
                column: "StoreId",
                principalTable: "store",
                principalColumn: "store_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "store_manager_staff_id_fkey",
                table: "store",
                column: "manager_staff_id",
                principalTable: "staff",
                principalColumn: "staff_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
