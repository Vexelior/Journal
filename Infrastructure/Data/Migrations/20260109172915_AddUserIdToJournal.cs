using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToJournal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add column as nullable first
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Journals",
                type: "nvarchar(450)",
                nullable: true);

            // Update existing journals with the admin user's ID
            // This will be handled by the application DbInitializer
            // or you can add SQL here if you know the admin user ID

            // Alter column to be required after data has been updated
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Journals",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Journals_UserId",
                table: "Journals",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Journals_AspNetUsers_UserId",
                table: "Journals",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Journals_AspNetUsers_UserId",
                table: "Journals");

            migrationBuilder.DropIndex(
                name: "IX_Journals_UserId",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Journals");
        }
    }
}
