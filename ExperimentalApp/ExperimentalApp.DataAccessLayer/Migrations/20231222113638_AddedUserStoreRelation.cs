using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExperimentalApp.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserStoreRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_store_StoreId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_store_StoreId",
                table: "AspNetUsers",
                column: "StoreId",
                principalTable: "store",
                principalColumn: "store_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_store_StoreId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_store_StoreId",
                table: "AspNetUsers",
                column: "StoreId",
                principalTable: "store",
                principalColumn: "store_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
