using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_YearlyJournals_Year",
                table: "YearlyJournals");

            migrationBuilder.AddColumn<int>(
                name: "JournalId",
                table: "YearlyJournals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JournalId",
                table: "Prompts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Journals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journals", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_YearlyJournals_JournalId_Year",
                table: "YearlyJournals",
                columns: new[] { "JournalId", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prompts_JournalId",
                table: "Prompts",
                column: "JournalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prompts_Journals_JournalId",
                table: "Prompts",
                column: "JournalId",
                principalTable: "Journals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_YearlyJournals_Journals_JournalId",
                table: "YearlyJournals",
                column: "JournalId",
                principalTable: "Journals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prompts_Journals_JournalId",
                table: "Prompts");

            migrationBuilder.DropForeignKey(
                name: "FK_YearlyJournals_Journals_JournalId",
                table: "YearlyJournals");

            migrationBuilder.DropTable(
                name: "Journals");

            migrationBuilder.DropIndex(
                name: "IX_YearlyJournals_JournalId_Year",
                table: "YearlyJournals");

            migrationBuilder.DropIndex(
                name: "IX_Prompts_JournalId",
                table: "Prompts");

            migrationBuilder.DropColumn(
                name: "JournalId",
                table: "YearlyJournals");

            migrationBuilder.DropColumn(
                name: "JournalId",
                table: "Prompts");

            migrationBuilder.CreateIndex(
                name: "IX_YearlyJournals_Year",
                table: "YearlyJournals",
                column: "Year",
                unique: true);
        }
    }
}
