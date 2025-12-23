using System.Globalization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Document = DocumentFormat.OpenXml.Wordprocessing.Document;

namespace Application.Services;

public class DocumentExportService
{
    public byte[] GeneratePdf(Journal journal)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(journal.Month);

        var document = QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

                page.Header()
                    .Text($"{monthName} {journal.Year} Journal")
                    .FontSize(20)
                    .Bold()
                    .AlignCenter();

                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        foreach (var entry in journal.JournalEntries.OrderBy(e => e.EntryDate))
                        {
                            column.Item().PaddingBottom(0.5f, Unit.Centimetre).Column(entryColumn =>
                            {
                                // Date
                                entryColumn.Item()
                                    .Text(entry.EntryDate.ToString("dddd, MMMM dd, yyyy"))
                                    .FontSize(14)
                                    .Bold();

                                // Prompt
                                if (entry.Prompt != null)
                                {
                                    entryColumn.Item()
                                        .PaddingTop(0.2f, Unit.Centimetre)
                                        .Text($"Prompt: {entry.Prompt.Text}")
                                        .FontSize(10)
                                        .Italic()
                                        .FontColor(Colors.Grey.Darken2);
                                }

                                // Content
                                entryColumn.Item()
                                    .PaddingTop(0.3f, Unit.Centimetre)
                                    .Text(entry.Content)
                                    .LineHeight(1.5f);

                                // Separator
                                entryColumn.Item()
                                    .PaddingTop(0.5f, Unit.Centimetre)
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten2);
                            });
                        }
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
            });
        });

        using var memoryStream = new MemoryStream();
        document.GeneratePdf(memoryStream);
        return memoryStream.ToArray();
    }
}