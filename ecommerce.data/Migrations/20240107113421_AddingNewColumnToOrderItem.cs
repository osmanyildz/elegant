using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerce.data.Migrations
{
    /// <inheritdoc />
    public partial class AddingNewColumnToOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsCancelledState",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SizeType",
                table: "OrderItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCancelledState",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "SizeType",
                table: "OrderItems");
        }
    }
}
