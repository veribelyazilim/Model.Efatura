using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Efatura.Migrations
{
    /// <inheritdoc />
    public partial class HtLogsErrorMsg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "HtLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "HtLogs");
        }
    }
}
