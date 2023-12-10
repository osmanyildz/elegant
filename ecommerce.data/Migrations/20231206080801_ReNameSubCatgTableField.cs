using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerce.data.Migrations
{
    /// <inheritdoc />
    public partial class ReNameSubCatgTableField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "subCategory",
                table: "SubCategories",
                newName: "subCategoryName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "subCategoryName",
                table: "SubCategories",
                newName: "subCategory");
        }
    }
}
