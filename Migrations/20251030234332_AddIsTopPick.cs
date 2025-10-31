using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesHub.Migrations
{
    /// <inheritdoc />
    public partial class AddIsTopPick : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTopPick",
                table: "Movies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTopPick",
                table: "Movies");
        }
    }
}
