using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerce.data.Migrations
{
    /// <inheritdoc />
    public partial class AddingSizeTypeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SizeType",
                table: "CartItems",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SizeType",
                table: "CartItems");
        }
    }
}
