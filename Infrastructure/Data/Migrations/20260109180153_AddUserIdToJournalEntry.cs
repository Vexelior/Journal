using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToJournalEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_Journals_JournalId",
                table: "JournalEntries");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "JournalEntries",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            // Update existing JournalEntries to have the same UserId as their parent Journal
            migrationBuilder.Sql(@"
                UPDATE je
                SET je.UserId = j.UserId
                FROM JournalEntries je
                INNER JOIN Journals j ON je.JournalId = j.Id
                WHERE je.UserId = ''
            ");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_UserId",
                table: "JournalEntries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_AspNetUsers_UserId",
                table: "JournalEntries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_Journals_JournalId",
                table: "JournalEntries",
                column: "JournalId",
                principalTable: "Journals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_AspNetUsers_UserId",
                table: "JournalEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_Journals_JournalId",
                table: "JournalEntries");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_UserId",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "JournalEntries");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_Journals_JournalId",
                table: "JournalEntries",
                column: "JournalId",
                principalTable: "Journals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
