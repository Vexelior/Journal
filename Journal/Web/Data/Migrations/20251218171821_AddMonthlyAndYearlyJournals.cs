using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMonthlyAndYearlyJournals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MonthlyJournalId",
                table: "JournalEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "YearlyJournals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearlyJournals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyJournals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    YearlyJournalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyJournals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyJournals_YearlyJournals_YearlyJournalId",
                        column: x => x.YearlyJournalId,
                        principalTable: "YearlyJournals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_MonthlyJournalId",
                table: "JournalEntries",
                column: "MonthlyJournalId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyJournals_YearlyJournalId_Month",
                table: "MonthlyJournals",
                columns: new[] { "YearlyJournalId", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_YearlyJournals_Year",
                table: "YearlyJournals",
                column: "Year",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_MonthlyJournals_MonthlyJournalId",
                table: "JournalEntries",
                column: "MonthlyJournalId",
                principalTable: "MonthlyJournals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_MonthlyJournals_MonthlyJournalId",
                table: "JournalEntries");

            migrationBuilder.DropTable(
                name: "MonthlyJournals");

            migrationBuilder.DropTable(
                name: "YearlyJournals");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_MonthlyJournalId",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "MonthlyJournalId",
                table: "JournalEntries");
        }
    }
}
