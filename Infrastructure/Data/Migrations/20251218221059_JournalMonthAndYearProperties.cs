using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class JournalMonthAndYearProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_MonthlyJournals_MonthlyJournalId",
                table: "JournalEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Prompts_Journals_JournalId",
                table: "Prompts");

            migrationBuilder.DropTable(
                name: "MonthlyJournals");

            migrationBuilder.DropTable(
                name: "YearlyJournals");

            migrationBuilder.DropIndex(
                name: "IX_Prompts_JournalId",
                table: "Prompts");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_EntryDate",
                table: "JournalEntries");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_MonthlyJournalId",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "JournalId",
                table: "Prompts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Journals");

            migrationBuilder.RenameColumn(
                name: "MonthlyJournalId",
                table: "JournalEntries",
                newName: "JournalId");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Prompts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Prompts",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Journals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Journals",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Journals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.CreateIndex(
                name: "IX_Journals_UserId_Year_Month",
                table: "Journals",
                columns: new[] { "UserId", "Year", "Month" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_JournalId_PromptId_EntryDate",
                table: "JournalEntries",
                columns: new[] { "JournalId", "PromptId", "EntryDate" });

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_Journals_JournalId",
                table: "JournalEntries",
                column: "JournalId",
                principalTable: "Journals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalEntries_Journals_JournalId",
                table: "JournalEntries");

            migrationBuilder.DropIndex(
                name: "IX_Journals_UserId_Year_Month",
                table: "Journals");

            migrationBuilder.DropIndex(
                name: "IX_JournalEntries_JournalId_PromptId_EntryDate",
                table: "JournalEntries");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Journals");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Journals");

            migrationBuilder.RenameColumn(
                name: "JournalId",
                table: "JournalEntries",
                newName: "MonthlyJournalId");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Prompts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Prompts",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "JournalId",
                table: "Prompts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Journals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "YearlyJournals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JournalId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearlyJournals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YearlyJournals_Journals_JournalId",
                        column: x => x.JournalId,
                        principalTable: "Journals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyJournals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YearlyJournalId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "IX_Prompts_JournalId",
                table: "Prompts",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntries_EntryDate",
                table: "JournalEntries",
                column: "EntryDate");

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
                name: "IX_YearlyJournals_JournalId_Year",
                table: "YearlyJournals",
                columns: new[] { "JournalId", "Year" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JournalEntries_MonthlyJournals_MonthlyJournalId",
                table: "JournalEntries",
                column: "MonthlyJournalId",
                principalTable: "MonthlyJournals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prompts_Journals_JournalId",
                table: "Prompts",
                column: "JournalId",
                principalTable: "Journals",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
