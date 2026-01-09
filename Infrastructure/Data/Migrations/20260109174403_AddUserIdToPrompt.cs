using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToPrompt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add column as nullable first
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Prompts",
                type: "nvarchar(450)",
                nullable: true);

            // Existing prompts will be updated by DbInitializer

            // Alter column to be required
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Prompts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prompts_UserId",
                table: "Prompts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prompts_AspNetUsers_UserId",
                table: "Prompts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prompts_AspNetUsers_UserId",
                table: "Prompts");

            migrationBuilder.DropIndex(
                name: "IX_Prompts_UserId",
                table: "Prompts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Prompts");
        }
    }
}
