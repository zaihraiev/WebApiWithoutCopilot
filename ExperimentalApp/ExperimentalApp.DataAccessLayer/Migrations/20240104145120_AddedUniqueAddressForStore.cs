using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExperimentalApp.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniqueAddressForStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_store_address_id",
                table: "store",
                column: "address_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_store_address_id",
                table: "store");
        }
    }
}
